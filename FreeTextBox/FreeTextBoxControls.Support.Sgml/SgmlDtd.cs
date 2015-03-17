using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
namespace FreeTextBoxControls.Support.Sgml
{
	public class SgmlDtd
	{
		public string Name;
		private Hashtable elements;
		private Hashtable pentities;
		private Hashtable entities;
		private StringBuilder sb;
		private Entity current;
		private XmlNameTable nameTable;
		private static string WhiteSpace = " \r\n\t";
		private static string ngterm = " \r\n\t|,)";
		private static string dcterm = " \r\n\t>";
		private static string cmterm = " \r\n\t,&|()?+*";
		private static string peterm = " \t\r\n>";
		public XmlNameTable NameTable
		{
			get
			{
				return this.nameTable;
			}
		}
		public SgmlDtd(string name, XmlNameTable nt)
		{
			this.nameTable = nt;
			this.Name = name;
			this.elements = new Hashtable();
			this.pentities = new Hashtable();
			this.entities = new Hashtable();
			this.sb = new StringBuilder();
		}
		public static SgmlDtd Parse(Uri baseUri, string name, string pubid, string url, string subset, string proxy, XmlNameTable nt)
		{
			SgmlDtd sgmlDtd = new SgmlDtd(name, nt);
			if (url != null && url != "")
			{
				sgmlDtd.PushEntity(baseUri, new Entity(sgmlDtd.Name, pubid, url, proxy));
			}
			if (subset != null && subset != "")
			{
				sgmlDtd.PushEntity(baseUri, new Entity(name, subset));
			}
			try
			{
				sgmlDtd.Parse();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message + sgmlDtd.current.Context());
			}
			return sgmlDtd;
		}
		public static SgmlDtd Parse(Uri baseUri, string name, string pubid, TextReader input, string subset, string proxy, XmlNameTable nt)
		{
			SgmlDtd sgmlDtd = new SgmlDtd(name, nt);
			sgmlDtd.PushEntity(baseUri, new Entity(sgmlDtd.Name, baseUri, input, proxy));
			if (subset != null && subset != "")
			{
				sgmlDtd.PushEntity(baseUri, new Entity(name, subset));
			}
			try
			{
				sgmlDtd.Parse();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message + sgmlDtd.current.Context());
			}
			return sgmlDtd;
		}
		public Entity FindEntity(string name)
		{
			return (Entity)this.entities[name];
		}
		public ElementDecl FindElement(string name)
		{
			return (ElementDecl)this.elements[name.ToUpper()];
		}
		private void PushEntity(Uri baseUri, Entity e)
		{
			e.Open(this.current, baseUri);
			this.current = e;
			this.current.ReadChar();
		}
		private void PopEntity()
		{
			if (this.current != null)
			{
				this.current.Close();
			}
			if (this.current.Parent != null)
			{
				this.current = this.current.Parent;
				return;
			}
			this.current = null;
		}
		private void Parse()
		{
			char c = this.current.Lastchar;
			while (true)
			{
				char c2 = c;
				if (c2 <= ' ')
				{
					switch (c2)
					{
					case '\t':
					case '\n':
					case '\r':
						break;
					case '\v':
					case '\f':
						goto IL_E0;
					default:
						if (c2 != ' ')
						{
							goto IL_E0;
						}
						break;
					}
					c = this.current.ReadChar();
					continue;
				}
				if (c2 == '%')
				{
					Entity e = this.ParseParameterEntity(SgmlDtd.WhiteSpace);
					try
					{
						this.PushEntity(this.current.ResolvedUri, e);
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message + this.current.Context());
					}
					c = this.current.Lastchar;
					continue;
				}
				if (c2 == '<')
				{
					this.ParseMarkup();
					c = this.current.ReadChar();
					continue;
				}
				if (c2 == '￿')
				{
					this.PopEntity();
					if (this.current == null)
					{
						break;
					}
					c = this.current.Lastchar;
					continue;
				}
				IL_E0:
				this.current.Error("Unexpected character '{0}'", c);
			}
		}
		private void ParseMarkup()
		{
			char c = this.current.ReadChar();
			if (c != '!')
			{
				this.current.Error("Found '{0}', but expecing declaration starting with '<!'");
				return;
			}
			c = this.current.ReadChar();
			if (c == '-')
			{
				c = this.current.ReadChar();
				if (c != '-')
				{
					this.current.Error("Expecting comment '<!--' but found {0}", c);
				}
				this.current.ScanToEnd(this.sb, "Comment", "-->");
				return;
			}
			if (c == '[')
			{
				this.ParseMarkedSection();
				return;
			}
			string text = this.current.ScanToken(this.sb, SgmlDtd.WhiteSpace, true);
			string a;
			if ((a = text) != null)
			{
				if (a == "ENTITY")
				{
					this.ParseEntity();
					return;
				}
				if (a == "ELEMENT")
				{
					this.ParseElementDecl();
					return;
				}
				if (a == "ATTLIST")
				{
					this.ParseAttList();
					return;
				}
			}
			this.current.Error("Invalid declaration '<!{0}'.  Expecting 'ENTITY', 'ELEMENT' or 'ATTLIST'.", text);
		}
		private char ParseDeclComments()
		{
			char c;
			for (c = this.current.Lastchar; c == '-'; c = this.ParseDeclComment(true))
			{
			}
			return c;
		}
		private char ParseDeclComment(bool full)
		{
			int arg_0B_0 = this.current.Line;
			char c = this.current.ReadChar();
			if (full && c != '-')
			{
				this.current.Error("Expecting comment delimiter '--' but found {0}", c);
			}
			this.current.ScanToEnd(this.sb, "Markup Comment", "--");
			return this.current.SkipWhitespace();
		}
		private void ParseMarkedSection()
		{
			this.current.ReadChar();
			string text = this.ScanName("[");
			if (text == "INCLUDE")
			{
				this.ParseIncludeSection();
				return;
			}
			if (text == "IGNORE")
			{
				this.ParseIgnoreSection();
				return;
			}
			this.current.Error("Unsupported marked section type '{0}'", text);
		}
		private void ParseIncludeSection()
		{
			throw new NotImplementedException("Include Section");
		}
		private void ParseIgnoreSection()
		{
			int arg_0B_0 = this.current.Line;
			char c = this.current.SkipWhitespace();
			if (c != '[')
			{
				this.current.Error("Expecting '[' but found {0}", c);
			}
			this.current.ScanToEnd(this.sb, "Conditional Section", "]]>");
		}
		private string ScanName(string term)
		{
			char c = this.current.SkipWhitespace();
			if (c != '%')
			{
				return this.current.ScanToken(this.sb, term, true);
			}
			Entity entity = this.ParseParameterEntity(term);
			c = this.current.Lastchar;
			if (!entity.Internal)
			{
				throw new NotSupportedException("External parameter entity resolution");
			}
			return entity.Literal.Trim();
		}
		private Entity ParseParameterEntity(string term)
		{
			this.current.ReadChar();
			string text = this.current.ScanToken(this.sb, ";" + term, false);
			text = this.nameTable.Add(text);
			if (this.current.Lastchar == ';')
			{
				this.current.ReadChar();
			}
			return this.GetParameterEntity(text);
		}
		private Entity GetParameterEntity(string name)
		{
			Entity entity = (Entity)this.pentities[name];
			if (entity == null)
			{
				this.current.Error("Reference to undefined parameter entity '{0}'", name);
			}
			return entity;
		}
		private void ParseEntity()
		{
			char c = this.current.SkipWhitespace();
			bool flag = c == '%';
			if (flag)
			{
				this.current.ReadChar();
				c = this.current.SkipWhitespace();
			}
			string text = this.current.ScanToken(this.sb, SgmlDtd.WhiteSpace, true);
			text = this.nameTable.Add(text);
			c = this.current.SkipWhitespace();
			Entity entity;
			if (c == '"' || c == '\'')
			{
				string literal = this.current.ScanLiteral(this.sb, c);
				entity = new Entity(text, literal);
			}
			else
			{
				string pubid = null;
				string text2 = this.current.ScanToken(this.sb, SgmlDtd.WhiteSpace, true);
				if (Entity.IsLiteralType(text2))
				{
					c = this.current.SkipWhitespace();
					string literal2 = this.current.ScanLiteral(this.sb, c);
					entity = new Entity(text, literal2);
					entity.SetLiteralType(text2);
				}
				else
				{
					string text3 = text2;
					if (text3 == "PUBLIC")
					{
						c = this.current.SkipWhitespace();
						if (c == '"' || c == '\'')
						{
							pubid = this.current.ScanLiteral(this.sb, c);
						}
						else
						{
							this.current.Error("Expecting public identifier literal but found '{0}'", c);
						}
					}
					else
					{
						if (text3 != "SYSTEM")
						{
							this.current.Error("Invalid external identifier '{0}'.  Expecing 'PUBLIC' or 'SYSTEM'.", text3);
						}
					}
					string uri = null;
					c = this.current.SkipWhitespace();
					if (c == '"' || c == '\'')
					{
						uri = this.current.ScanLiteral(this.sb, c);
					}
					else
					{
						if (c != '>')
						{
							this.current.Error("Expecting system identifier literal but found '{0}'", c);
						}
					}
					entity = new Entity(text, pubid, uri, this.current.Proxy);
				}
			}
			c = this.current.SkipWhitespace();
			if (c == '-')
			{
				c = this.ParseDeclComments();
			}
			if (c != '>')
			{
				this.current.Error("Expecting end of entity declaration '>' but found '{0}'", c);
			}
			if (flag)
			{
				this.pentities.Add(entity.Name, entity);
				return;
			}
			this.entities.Add(entity.Name, entity);
		}
		private void ParseElementDecl()
		{
			char c = this.current.SkipWhitespace();
			string[] array = this.ParseNameGroup(c, true);
			bool sto = char.ToLower(this.current.SkipWhitespace()) == 'o';
			this.current.ReadChar();
			bool eto = char.ToLower(this.current.SkipWhitespace()) == 'o';
			this.current.ReadChar();
			c = this.current.SkipWhitespace();
			ContentModel cm = this.ParseContentModel(c);
			c = this.current.SkipWhitespace();
			string[] exclusions = null;
			string[] inclusions = null;
			if (c == '-')
			{
				c = this.current.ReadChar();
				if (c == '(')
				{
					exclusions = this.ParseNameGroup(c, true);
					c = this.current.SkipWhitespace();
				}
				else
				{
					if (c == '-')
					{
						c = this.ParseDeclComment(false);
					}
					else
					{
						this.current.Error("Invalid syntax at '{0}'", c);
					}
				}
			}
			if (c == '-')
			{
				c = this.ParseDeclComments();
			}
			if (c == '+')
			{
				c = this.current.ReadChar();
				if (c != '(')
				{
					this.current.Error("Expecting inclusions name group", c);
				}
				inclusions = this.ParseNameGroup(c, true);
				c = this.current.SkipWhitespace();
			}
			if (c == '-')
			{
				c = this.ParseDeclComments();
			}
			if (c != '>')
			{
				this.current.Error("Expecting end of ELEMENT declaration '>' but found '{0}'", c);
			}
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				string text2 = text.ToUpper();
				text2 = this.nameTable.Add(text);
				this.elements.Add(text2, new ElementDecl(text2, sto, eto, cm, inclusions, exclusions));
			}
		}
		private string[] ParseNameGroup(char ch, bool nmtokens)
		{
			ArrayList arrayList = new ArrayList();
			if (ch == '(')
			{
				ch = this.current.ReadChar();
				ch = this.current.SkipWhitespace();
				while (ch != ')')
				{
					ch = this.current.SkipWhitespace();
					if (ch == '%')
					{
						Entity e = this.ParseParameterEntity(SgmlDtd.ngterm);
						this.PushEntity(this.current.ResolvedUri, e);
						this.ParseNameList(arrayList, nmtokens);
						this.PopEntity();
						ch = this.current.Lastchar;
					}
					else
					{
						string text = this.current.ScanToken(this.sb, SgmlDtd.ngterm, nmtokens);
						text = text.ToUpper();
						string value = this.nameTable.Add(text);
						arrayList.Add(value);
					}
					ch = this.current.SkipWhitespace();
					if (ch == '|' || ch == ',')
					{
						ch = this.current.ReadChar();
					}
				}
				this.current.ReadChar();
			}
			else
			{
				string text2 = this.current.ScanToken(this.sb, SgmlDtd.WhiteSpace, nmtokens);
				text2 = text2.ToUpper();
				text2 = this.nameTable.Add(text2);
				arrayList.Add(text2);
			}
			return (string[])arrayList.ToArray(typeof(string));
		}
		private void ParseNameList(ArrayList names, bool nmtokens)
		{
			char c = this.current.Lastchar;
			c = this.current.SkipWhitespace();
			while (c != '￿')
			{
				if (c == '%')
				{
					Entity e = this.ParseParameterEntity(SgmlDtd.ngterm);
					this.PushEntity(this.current.ResolvedUri, e);
					this.ParseNameList(names, nmtokens);
					this.PopEntity();
					c = this.current.Lastchar;
				}
				else
				{
					string text = this.current.ScanToken(this.sb, SgmlDtd.ngterm, true);
					text = text.ToUpper();
					text = this.nameTable.Add(text);
					names.Add(text);
				}
				c = this.current.SkipWhitespace();
				if (c == '|')
				{
					c = this.current.ReadChar();
					c = this.current.SkipWhitespace();
				}
			}
		}
		private ContentModel ParseContentModel(char ch)
		{
			ContentModel contentModel = new ContentModel();
			if (ch == '(')
			{
				this.current.ReadChar();
				this.ParseModel(')', contentModel);
				ch = this.current.ReadChar();
				if (ch == '?' || ch == '+' || ch == '*')
				{
					contentModel.AddOccurrence(ch);
					this.current.ReadChar();
				}
			}
			else
			{
				if (ch == '%')
				{
					Entity e = this.ParseParameterEntity(SgmlDtd.dcterm);
					this.PushEntity(this.current.ResolvedUri, e);
					contentModel = this.ParseContentModel(this.current.Lastchar);
					this.PopEntity();
				}
				else
				{
					string declaredContent = this.ScanName(SgmlDtd.dcterm);
					contentModel.SetDeclaredContent(declaredContent);
				}
			}
			return contentModel;
		}
		private void ParseModel(char cmt, ContentModel cm)
		{
			int currentDepth = cm.CurrentDepth;
			char c = this.current.Lastchar;
			c = this.current.SkipWhitespace();
			while (c != cmt || cm.CurrentDepth > currentDepth)
			{
				if (c == '￿')
				{
					this.current.Error("Content Model was not closed");
				}
				if (c == '%')
				{
					Entity e = this.ParseParameterEntity(SgmlDtd.cmterm);
					this.PushEntity(this.current.ResolvedUri, e);
					this.ParseModel('￿', cm);
					this.PopEntity();
					c = this.current.SkipWhitespace();
				}
				else
				{
					if (c == '(')
					{
						cm.PushGroup();
						this.current.ReadChar();
						c = this.current.SkipWhitespace();
					}
					else
					{
						if (c == ')')
						{
							c = this.current.ReadChar();
							if (c == '*' || c == '+' || c == '?')
							{
								cm.AddOccurrence(c);
								c = this.current.ReadChar();
							}
							if (cm.PopGroup() < currentDepth)
							{
								this.current.Error("Parameter entity cannot close a paren outside it's own scope");
							}
							c = this.current.SkipWhitespace();
						}
						else
						{
							if (c == ',' || c == '|' || c == '&')
							{
								cm.AddConnector(c);
								this.current.ReadChar();
								c = this.current.SkipWhitespace();
							}
							else
							{
								string text;
								if (c == '#')
								{
									c = this.current.ReadChar();
									text = "#" + this.current.ScanToken(this.sb, SgmlDtd.cmterm, true);
								}
								else
								{
									text = this.current.ScanToken(this.sb, SgmlDtd.cmterm, true);
								}
								text = text.ToUpper();
								text = this.nameTable.Add(text);
								c = this.current.Lastchar;
								if (c == '?' || c == '+' || c == '*')
								{
									cm.PushGroup();
									cm.AddSymbol(text);
									cm.AddOccurrence(c);
									cm.PopGroup();
									this.current.ReadChar();
									c = this.current.SkipWhitespace();
								}
								else
								{
									cm.AddSymbol(text);
									c = this.current.SkipWhitespace();
								}
							}
						}
					}
				}
			}
		}
		private void ParseAttList()
		{
			char ch = this.current.SkipWhitespace();
			string[] array = this.ParseNameGroup(ch, true);
			AttList list = new AttList();
			this.ParseAttList(list, '>');
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				ElementDecl elementDecl = (ElementDecl)this.elements[text];
				if (elementDecl == null)
				{
					this.current.Error("ATTLIST references undefined ELEMENT {0}", text);
				}
				elementDecl.AddAttDefs(list);
			}
		}
		private void ParseAttList(AttList list, char term)
		{
			for (char c = this.current.SkipWhitespace(); c != term; c = this.current.SkipWhitespace())
			{
				if (c == '%')
				{
					Entity e = this.ParseParameterEntity(SgmlDtd.peterm);
					this.PushEntity(this.current.ResolvedUri, e);
					this.ParseAttList(list, '￿');
					this.PopEntity();
					c = this.current.SkipWhitespace();
				}
				else
				{
					if (c == '-')
					{
						c = this.ParseDeclComments();
					}
					else
					{
						AttDef a = this.ParseAttDef(c);
						list.Add(a);
					}
				}
			}
		}
		private AttDef ParseAttDef(char ch)
		{
			ch = this.current.SkipWhitespace();
			string text = this.ScanName(SgmlDtd.WhiteSpace);
			text = text.ToUpper();
			text = this.nameTable.Add(text);
			AttDef attDef = new AttDef(text);
			ch = this.current.SkipWhitespace();
			if (ch == '-')
			{
				ch = this.ParseDeclComments();
			}
			this.ParseAttType(ch, attDef);
			ch = this.current.SkipWhitespace();
			if (ch == '-')
			{
				ch = this.ParseDeclComments();
			}
			this.ParseAttDefault(ch, attDef);
			ch = this.current.SkipWhitespace();
			if (ch == '-')
			{
				ch = this.ParseDeclComments();
			}
			return attDef;
		}
		private void ParseAttType(char ch, AttDef attdef)
		{
			if (ch == '%')
			{
				Entity e = this.ParseParameterEntity(SgmlDtd.WhiteSpace);
				this.PushEntity(this.current.ResolvedUri, e);
				this.ParseAttType(this.current.Lastchar, attdef);
				this.PopEntity();
				ch = this.current.Lastchar;
				return;
			}
			if (ch == '(')
			{
				attdef.EnumValues = this.ParseNameGroup(ch, false);
				attdef.Type = AttributeType.ENUMERATION;
				return;
			}
			string text = this.ScanName(SgmlDtd.WhiteSpace);
			if (text == "NOTATION")
			{
				ch = this.current.SkipWhitespace();
				if (ch != '(')
				{
					this.current.Error("Expecting name group '(', but found '{0}'", ch);
				}
				attdef.Type = AttributeType.NOTATION;
				attdef.EnumValues = this.ParseNameGroup(ch, true);
				return;
			}
			attdef.SetType(text);
		}
		private void ParseAttDefault(char ch, AttDef attdef)
		{
			if (ch == '%')
			{
				Entity e = this.ParseParameterEntity(SgmlDtd.WhiteSpace);
				this.PushEntity(this.current.ResolvedUri, e);
				this.ParseAttDefault(this.current.Lastchar, attdef);
				this.PopEntity();
				ch = this.current.Lastchar;
				return;
			}
			bool flag = true;
			if (ch == '#')
			{
				this.current.ReadChar();
				string presence = this.current.ScanToken(this.sb, SgmlDtd.WhiteSpace, true);
				flag = attdef.SetPresence(presence);
				ch = this.current.SkipWhitespace();
			}
			if (flag)
			{
				if (ch == '\'' || ch == '"')
				{
					string @default = this.current.ScanLiteral(this.sb, ch);
					attdef.Default = @default;
					ch = this.current.SkipWhitespace();
					return;
				}
				string text = this.current.ScanToken(this.sb, SgmlDtd.WhiteSpace, false);
				text = text.ToUpper();
				text = this.nameTable.Add(text);
				attdef.Default = text;
				ch = this.current.SkipWhitespace();
			}
		}
	}
}
