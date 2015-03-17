using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
namespace FreeTextBoxControls.Support.Sgml
{
	public class SgmlReader : XmlReader
	{
		private SgmlDtd dtd;
		private Entity current;
		private State state;
		private XmlNameTable nametable;
		private char partial;
		private object endTag;
		private HWStack stack;
		private Node node;
		private Attribute a;
		private int apos;
		private Uri baseUri;
		private StringBuilder sb;
		private StringBuilder name;
		private TextWriter log;
		private Node newnode;
		private int poptodepth;
		private int rootCount;
		private bool isHtml;
		private string rootElementName;
		private string href;
		private string errorLogFile;
		private Entity lastError;
		private string proxy;
		private TextReader inputStream;
		private string syslit;
		private string pubid;
		private string subset;
		private string docType;
		private WhitespaceHandling whitespaceHandling;
		private CaseFolding folding;
		private bool stripDocType = true;
		private string startTag;
		private static string declterm = " \t\r\n><";
		private static string tagterm = " \t\r\n=/><";
		private static string aterm = " \t\r\n=/>";
		private static string avterm = " \t\r\n>";
		private static string cdataterm = "\t\r\n[<>";
		private static string dtterm = " \t\r\n>";
		private static string piterm = " \t\r\n?";
		public SgmlDtd Dtd
		{
			get
			{
				this.LazyLoadDtd(this.baseUri);
				return this.dtd;
			}
			set
			{
				this.dtd = value;
			}
		}
		public string DocType
		{
			get
			{
				return this.docType;
			}
			set
			{
				this.docType = value;
			}
		}
		public string PublicIdentifier
		{
			get
			{
				return this.pubid;
			}
			set
			{
				this.pubid = value;
			}
		}
		public string SystemLiteral
		{
			get
			{
				return this.syslit;
			}
			set
			{
				this.syslit = value;
			}
		}
		public string InternalSubset
		{
			get
			{
				return this.subset;
			}
			set
			{
				this.subset = value;
			}
		}
		public TextReader InputStream
		{
			get
			{
				return this.inputStream;
			}
			set
			{
				this.inputStream = value;
				this.Init();
			}
		}
		public string WebProxy
		{
			get
			{
				return this.proxy;
			}
			set
			{
				this.proxy = value;
			}
		}
		public string Href
		{
			get
			{
				return this.href;
			}
			set
			{
				this.href = value;
				this.Init();
				if (this.baseUri == null)
				{
					if (this.href.IndexOf("://") > 0)
					{
						this.baseUri = new Uri(this.href);
						return;
					}
					this.baseUri = new Uri("file:///" + Directory.GetCurrentDirectory() + "//");
				}
			}
		}
		public bool StripDocType
		{
			get
			{
				return this.stripDocType;
			}
			set
			{
				this.stripDocType = value;
			}
		}
		public CaseFolding CaseFolding
		{
			get
			{
				return this.folding;
			}
			set
			{
				this.folding = value;
			}
		}
		public TextWriter ErrorLog
		{
			get
			{
				return this.log;
			}
			set
			{
				this.log = value;
			}
		}
		public string ErrorLogFile
		{
			get
			{
				return this.errorLogFile;
			}
			set
			{
				this.errorLogFile = value;
				this.ErrorLog = new StreamWriter(value);
			}
		}
		public override XmlNodeType NodeType
		{
			get
			{
				if (this.state == State.Attr)
				{
					return XmlNodeType.Attribute;
				}
				if (this.state == State.AttrValue)
				{
					return XmlNodeType.Text;
				}
				if (this.state == State.EndTag || this.state == State.AutoClose)
				{
					return XmlNodeType.EndElement;
				}
				return this.node.NodeType;
			}
		}
		public override string Name
		{
			get
			{
				return this.LocalName;
			}
		}
		public override string LocalName
		{
			get
			{
				string result;
				if (this.state == State.Attr)
				{
					result = this.a.Name;
				}
				else
				{
					if (this.state == State.AttrValue)
					{
						result = null;
					}
					else
					{
						result = this.node.Name;
					}
				}
				return result;
			}
		}
		public override string NamespaceURI
		{
			get
			{
				if (this.state == State.Attr && this.a.Name == "xmlns")
				{
					return "http://www.w3.org/2000/xmlns/";
				}
				return string.Empty;
			}
		}
		public override string Prefix
		{
			get
			{
				return string.Empty;
			}
		}
		public override bool HasValue
		{
			get
			{
				return this.state == State.Attr || this.state == State.AttrValue || this.node.Value != null;
			}
		}
		public override string Value
		{
			get
			{
				if (this.state == State.Attr || this.state == State.AttrValue)
				{
					return this.a.Value;
				}
				return this.node.Value;
			}
		}
		public override int Depth
		{
			get
			{
				if (this.state == State.Attr)
				{
					return this.stack.Count;
				}
				if (this.state == State.AttrValue)
				{
					return this.stack.Count + 1;
				}
				return this.stack.Count - 1;
			}
		}
		public override string BaseURI
		{
			get
			{
				if (!(this.baseUri == null))
				{
					return this.baseUri.AbsoluteUri;
				}
				return "";
			}
		}
		public override bool IsEmptyElement
		{
			get
			{
				return (this.state == State.Markup || this.state == State.Attr || this.state == State.AttrValue) && this.node.IsEmpty;
			}
		}
		public override bool IsDefault
		{
			get
			{
				return (this.state == State.Attr || this.state == State.AttrValue) && this.a.IsDefault;
			}
		}
		public override char QuoteChar
		{
			get
			{
				if (this.a != null)
				{
					return this.a.QuoteChar;
				}
				return '\0';
			}
		}
		public override XmlSpace XmlSpace
		{
			get
			{
				for (int i = this.stack.Count - 1; i > 1; i--)
				{
					Node node = (Node)this.stack[i];
					XmlSpace space = node.Space;
					if (space != XmlSpace.None)
					{
						return space;
					}
				}
				return XmlSpace.None;
			}
		}
		public override string XmlLang
		{
			get
			{
				for (int i = this.stack.Count - 1; i > 1; i--)
				{
					Node node = (Node)this.stack[i];
					string xmlLang = node.XmlLang;
					if (xmlLang != null)
					{
						return xmlLang;
					}
				}
				return string.Empty;
			}
		}
		public WhitespaceHandling WhitespaceHandling
		{
			get
			{
				return this.whitespaceHandling;
			}
			set
			{
				this.whitespaceHandling = value;
			}
		}
		public override int AttributeCount
		{
			get
			{
				if (this.state == State.Attr || this.state == State.AttrValue)
				{
					return 0;
				}
				if (this.node.NodeType == XmlNodeType.Element || this.node.NodeType == XmlNodeType.DocumentType)
				{
					return this.node.AttributeCount;
				}
				return 0;
			}
		}
		public override string this[int i]
		{
			get
			{
				return this.GetAttribute(i);
			}
		}
		public override string this[string name]
		{
			get
			{
				return this.GetAttribute(name);
			}
		}
		public override string this[string name, string namespaceURI]
		{
			get
			{
				return this.GetAttribute(name, namespaceURI);
			}
		}
		private bool IsHtml
		{
			get
			{
				return this.isHtml;
			}
		}
		public override bool EOF
		{
			get
			{
				return this.state == State.Eof;
			}
		}
		public override ReadState ReadState
		{
			get
			{
				if (this.state == State.Initial)
				{
					return ReadState.Initial;
				}
				if (this.state == State.Eof)
				{
					return ReadState.EndOfFile;
				}
				return ReadState.Interactive;
			}
		}
		public override XmlNameTable NameTable
		{
			get
			{
				return this.nametable;
			}
		}
		public SgmlReader()
		{
			this.Init();
			this.nametable = new NameTable();
		}
		private void LazyLoadDtd(Uri baseUri)
		{
			if (this.dtd == null)
			{
				if (this.syslit == null || this.syslit == "")
				{
					if (this.docType == null || !(this.docType.ToLower() == "html"))
					{
						goto IL_143;
					}
					Assembly assembly = typeof(FreeTextBox).Assembly;
					string str = "FreeTextBoxControls.Support.SgmlReader.Resources.Html.dtd";
					Stream manifestResourceStream = assembly.GetManifestResourceStream(str);
					try
					{
						StreamReader input = new StreamReader(manifestResourceStream);
						this.dtd = SgmlDtd.Parse(baseUri, "HTML", null, input, null, this.proxy, this.nametable);
						goto IL_143;
					}
					catch (Exception ex)
					{
						throw new Exception(ex.ToString() + "//" + str + "\\\\");
					}
				}
				if (baseUri != null)
				{
					baseUri = new Uri(baseUri, this.syslit);
				}
				else
				{
					if (this.baseUri != null)
					{
						baseUri = new Uri(this.baseUri, this.syslit);
					}
					else
					{
						baseUri = new Uri(new Uri(Directory.GetCurrentDirectory() + "\\"), this.syslit);
					}
				}
				this.dtd = SgmlDtd.Parse(baseUri, this.docType, this.pubid, baseUri.AbsoluteUri, this.subset, this.proxy, this.nametable);
				IL_143:
				if (this.dtd != null && this.dtd.Name != null)
				{
					switch (this.CaseFolding)
					{
					case CaseFolding.ToUpper:
						this.rootElementName = this.dtd.Name.ToUpper();
						break;
					case CaseFolding.ToLower:
						this.rootElementName = this.dtd.Name.ToLower();
						break;
					default:
						this.rootElementName = this.dtd.Name;
						break;
					}
					this.isHtml = (this.dtd.Name.ToLower() == "html");
				}
			}
		}
		public void SetBaseUri(string uri)
		{
			this.baseUri = new Uri(uri);
		}
		private void Log(string msg, params string[] args)
		{
			if (this.ErrorLog != null)
			{
				string text = string.Format(msg, args);
				if (this.lastError != this.current)
				{
					text = text + "    " + this.current.Context();
					this.lastError = this.current;
					this.ErrorLog.WriteLine("### Error:" + text);
					return;
				}
				string text2 = "";
				if (this.current.ResolvedUri != null)
				{
					text2 = this.current.ResolvedUri.AbsolutePath;
				}
				this.ErrorLog.WriteLine(string.Concat(new object[]
				{
					"### Error in ",
					text2,
					"#",
					this.current.Name,
					", line ",
					this.current.Line,
					", position ",
					this.current.LinePosition,
					": ",
					text
				}));
			}
		}
		private void Log(string msg, char ch)
		{
			this.Log(msg, new string[]
			{
				ch.ToString()
			});
		}
		private void Init()
		{
			this.state = State.Initial;
			this.stack = new HWStack(10);
			this.node = this.Push(null, XmlNodeType.Document, null);
			this.node.IsEmpty = false;
			this.sb = new StringBuilder();
			this.name = new StringBuilder();
			this.poptodepth = 0;
			this.current = null;
			this.partial = '\0';
			this.endTag = null;
			this.a = null;
			this.apos = 0;
			this.newnode = null;
			this.rootCount = 0;
		}
		private Node Push(string name, XmlNodeType nt, string value)
		{
			Node node = (Node)this.stack.Push();
			if (node == null)
			{
				node = new Node();
				this.stack[this.stack.Count - 1] = node;
			}
			node.Reset(name, nt, value);
			this.node = node;
			return node;
		}
		private Node Push(Node n)
		{
			Node node = this.Push(n.Name, n.NodeType, n.Value);
			node.DtdType = n.DtdType;
			node.IsEmpty = n.IsEmpty;
			node.Space = n.Space;
			node.XmlLang = n.XmlLang;
			node.CurrentState = n.CurrentState;
			node.CopyAttributes(n);
			this.node = node;
			return node;
		}
		private void Pop()
		{
			if (this.stack.Count > 1)
			{
				this.node = (Node)this.stack.Pop();
			}
		}
		public override string GetAttribute(string name)
		{
			if (this.state != State.Attr && this.state != State.AttrValue)
			{
				int attribute = this.node.GetAttribute(name);
				if (attribute >= 0)
				{
					return this.GetAttribute(attribute);
				}
			}
			return null;
		}
		public override string GetAttribute(string name, string namespaceURI)
		{
			return this.GetAttribute(name);
		}
		public override string GetAttribute(int i)
		{
			if (this.state != State.Attr && this.state != State.AttrValue)
			{
				Attribute attribute = this.node.GetAttribute(i);
				if (attribute != null)
				{
					return attribute.Value;
				}
			}
			throw new IndexOutOfRangeException();
		}
		public override bool MoveToAttribute(string name)
		{
			int attribute = this.node.GetAttribute(name);
			if (attribute >= 0)
			{
				this.MoveToAttribute(attribute);
				return true;
			}
			return false;
		}
		public override bool MoveToAttribute(string name, string ns)
		{
			return this.MoveToAttribute(name);
		}
		public override void MoveToAttribute(int i)
		{
			Attribute attribute = this.node.GetAttribute(i);
			if (attribute != null)
			{
				this.apos = i;
				this.a = attribute;
				this.node.CurrentState = this.state;
				this.state = State.Attr;
				return;
			}
			throw new IndexOutOfRangeException();
		}
		public override bool MoveToFirstAttribute()
		{
			if (this.node.AttributeCount > 0)
			{
				this.MoveToAttribute(0);
				return true;
			}
			return false;
		}
		public override bool MoveToNextAttribute()
		{
			if (this.state != State.Attr && this.state != State.AttrValue)
			{
				return this.MoveToFirstAttribute();
			}
			if (this.apos < this.node.AttributeCount - 1)
			{
				this.MoveToAttribute(this.apos + 1);
				return true;
			}
			return false;
		}
		public override bool MoveToElement()
		{
			if (this.state == State.Attr || this.state == State.AttrValue)
			{
				this.state = this.node.CurrentState;
				this.a = null;
				return true;
			}
			return this.node.NodeType == XmlNodeType.Element;
		}
		public Encoding GetEncoding()
		{
			if (this.current == null)
			{
				this.OpenInput();
			}
			return this.current.GetEncoding();
		}
		private void OpenInput()
		{
			this.LazyLoadDtd(this.baseUri);
			if (this.Href != null)
			{
				this.current = new Entity("#document", null, this.href, this.proxy);
			}
			else
			{
				if (this.inputStream == null)
				{
					throw new InvalidOperationException("You must specify input either via Href or InputStream properties");
				}
				this.current = new Entity("#document", null, this.inputStream, this.proxy);
			}
			this.current.Html = this.IsHtml;
			this.current.Open(null, this.baseUri);
			if (this.current.ResolvedUri != null)
			{
				this.baseUri = this.current.ResolvedUri;
			}
		}
		public override bool Read()
		{
			if (this.current == null)
			{
				this.OpenInput();
			}
			bool flag = false;
			while (!flag)
			{
				switch (this.state)
				{
				case State.Initial:
					this.state = State.Markup;
					this.current.ReadChar();
					goto IL_CB;
				case State.Markup:
					goto IL_CB;
				case State.EndTag:
					if (this.endTag == this.node.Name)
					{
						this.Pop();
						this.state = State.Markup;
						goto IL_CB;
					}
					this.Pop();
					flag = true;
					break;
				case State.Attr:
				case State.AttrValue:
					this.state = State.Markup;
					goto IL_CB;
				case State.Text:
					this.Pop();
					goto IL_CB;
				case State.PartialTag:
					this.Pop();
					this.state = State.Markup;
					flag = this.ParseTag(this.partial);
					break;
				case State.AutoClose:
					this.Pop();
					if (this.stack.Count <= this.poptodepth)
					{
						this.state = State.Markup;
						this.Push(this.newnode);
						this.state = State.Markup;
					}
					flag = true;
					break;
				case State.CData:
					flag = this.ParseCData();
					break;
				case State.PartialText:
					if (this.ParseText(this.current.Lastchar, false))
					{
						this.node.NodeType = XmlNodeType.Whitespace;
					}
					flag = true;
					break;
				case State.PseudoStartTag:
					flag = this.ParseStartTag('<');
					break;
				case State.Eof:
					if (this.current.Parent == null)
					{
						return false;
					}
					this.current.Close();
					this.current = this.current.Parent;
					break;
				}
				IL_18F:
				if (flag && this.node.NodeType == XmlNodeType.Whitespace && this.whitespaceHandling == WhitespaceHandling.None)
				{
					flag = false;
					continue;
				}
				continue;
				IL_CB:
				if (this.node.IsEmpty)
				{
					this.Pop();
				}
				flag = this.ParseMarkup();
				goto IL_18F;
			}
			return true;
		}
		private bool ParseMarkup()
		{
			char c = this.current.Lastchar;
			if (c == '<')
			{
				c = this.current.ReadChar();
				return this.ParseTag(c);
			}
			if (c == '￿')
			{
				this.state = State.Eof;
				return false;
			}
			if (this.node.DtdType != null && this.node.DtdType.ContentModel.DeclaredContent == DeclaredContent.CDATA)
			{
				this.partial = '\0';
				this.state = State.CData;
				return false;
			}
			if (this.ParseText(c, true))
			{
				this.node.NodeType = XmlNodeType.Whitespace;
			}
			return true;
		}
		private bool ParseTag(char ch)
		{
			if (ch == '%')
			{
				return this.ParseAspNet();
			}
			if (ch == '!')
			{
				ch = this.current.ReadChar();
				if (ch == '-')
				{
					return this.ParseComment();
				}
				if (ch == '[')
				{
					return this.ParseConditionalBlock();
				}
				if (ch != '_' && !char.IsLetter(ch))
				{
					string str = this.current.ScanToEnd(this.sb, "Recovering", ">");
					this.Log("Ignoring invalid markup '<!" + str + ">", new string[0]);
					return false;
				}
				string text = this.current.ScanToken(this.sb, SgmlReader.declterm, false);
				if (!(text == "DOCTYPE"))
				{
					this.Log("Invalid declaration '<!{0}...'.  Expecting '<!DOCTYPE' only.", new string[]
					{
						text
					});
					this.current.ScanToEnd(null, "Recovering", ">");
					return false;
				}
				this.ParseDocType();
				if (this.GetAttribute("SYSTEM") == null && this.GetAttribute("PUBLIC") != null)
				{
					this.node.AddAttribute("SYSTEM", "", '"', this.folding == CaseFolding.None);
				}
				if (this.stripDocType)
				{
					return false;
				}
				this.node.NodeType = XmlNodeType.DocumentType;
				return true;
			}
			else
			{
				if (ch == '?')
				{
					this.current.ReadChar();
					this.ParsePI();
					return true;
				}
				if (ch == '/')
				{
					return this.ParseEndTag();
				}
				return this.ParseStartTag(ch);
			}
		}
		private string ScanName(string terminators)
		{
			string text = this.current.ScanToken(this.sb, terminators, false);
			switch (this.folding)
			{
			case CaseFolding.ToUpper:
				text = text.ToUpper();
				break;
			case CaseFolding.ToLower:
				text = text.ToLower();
				break;
			}
			return this.nametable.Add(text);
		}
		private bool ParseStartTag(char ch)
		{
			string text;
			if (this.state != State.PseudoStartTag)
			{
				if (SgmlReader.tagterm.IndexOf(ch) >= 0)
				{
					this.sb.Length = 0;
					this.sb.Append('<');
					this.state = State.PartialText;
					return false;
				}
				text = this.ScanName(SgmlReader.tagterm);
				if (this.IsHtml && this.Depth == 0 && text != "html" && text != "HTML")
				{
					Node node = this.Push("html", XmlNodeType.Element, null);
					node.IsEmpty = false;
					this.state = State.PseudoStartTag;
					this.startTag = text;
					return true;
				}
			}
			else
			{
				text = this.startTag;
				this.state = State.Markup;
			}
			Node node2 = this.Push(text, XmlNodeType.Element, null);
			node2.IsEmpty = false;
			this.Validate(node2);
			ch = this.current.SkipWhitespace();
			while (ch != '￿' && ch != '>')
			{
				if (ch == '/')
				{
					node2.IsEmpty = true;
					ch = this.current.ReadChar();
					if (ch != '>')
					{
						this.Log("Expected empty start tag '/>' sequence instead of '{0}'", ch);
						this.current.ScanToEnd(null, "Recovering", ">");
						return false;
					}
					break;
				}
				else
				{
					if (ch == '<')
					{
						this.Log("Start tag '{0}' is missing '>'", new string[]
						{
							text
						});
						break;
					}
					string text2 = this.ScanName(SgmlReader.aterm);
					ch = this.current.SkipWhitespace();
					string value = null;
					char quotechar = '\0';
					if (ch == '=')
					{
						this.current.ReadChar();
						ch = this.current.SkipWhitespace();
						if (ch == '\'' || ch == '"')
						{
							quotechar = ch;
							value = this.ScanLiteral(this.sb, ch);
						}
						else
						{
							if (ch != '>')
							{
								string term = SgmlReader.avterm;
								value = this.current.ScanToken(this.sb, term, false);
							}
						}
					}
					if (text2.Length > 0)
					{
						Attribute attribute = node2.AddAttribute(text2, value, quotechar, this.folding == CaseFolding.None);
						if (attribute == null)
						{
							this.Log("Duplicate attribute '{0}' ignored", new string[]
							{
								text2
							});
						}
						else
						{
							this.ValidateAttribute(node2, attribute);
						}
					}
					ch = this.current.SkipWhitespace();
				}
			}
			if (ch == '￿')
			{
				this.current.Error("Unexpected EOF parsing start tag '{0}'", text);
			}
			else
			{
				if (ch == '>')
				{
					this.current.ReadChar();
				}
			}
			if (this.Depth == 1)
			{
				if (this.rootCount == 1)
				{
					this.state = State.Eof;
					return false;
				}
				this.rootCount++;
			}
			this.ValidateContent(node2);
			return true;
		}
		private bool ParseEndTag()
		{
			this.state = State.EndTag;
			this.current.ReadChar();
			string text = this.ScanName(SgmlReader.tagterm);
			char c = this.current.SkipWhitespace();
			if (c != '>')
			{
				this.Log("Expected empty start tag '/>' sequence instead of '{0}'", c);
				this.current.ScanToEnd(null, "Recovering", ">");
			}
			this.current.ReadChar();
			this.endTag = text;
			bool flag = this.folding == CaseFolding.None;
			this.node = (Node)this.stack[this.stack.Count - 1];
			for (int i = this.stack.Count - 1; i > 0; i--)
			{
				Node node = (Node)this.stack[i];
				if (flag && string.Compare(node.Name, text, true) == 0)
				{
					this.endTag = node.Name;
					return true;
				}
				if (node.Name == text)
				{
					return true;
				}
			}
			this.Log("No matching start tag for '</{0}>'", new string[]
			{
				text
			});
			this.state = State.Markup;
			return false;
		}
		private bool ParseAspNet()
		{
			string value = "<%" + this.current.ScanToEnd(this.sb, "AspNet", "%>") + "%>";
			this.Push(null, XmlNodeType.CDATA, value);
			return true;
		}
		private bool ParseComment()
		{
			char c = this.current.ReadChar();
			if (c != '-')
			{
				this.Log("Expecting comment '<!--' but found {0}", c);
				this.current.ScanToEnd(null, "Comment", ">");
				return false;
			}
			string text = this.current.ScanToEnd(this.sb, "Comment", "-->");
			for (int i = text.IndexOf("--"); i >= 0; i = text.IndexOf("--"))
			{
				int num = i + 2;
				while (num < text.Length && text[num] == '-')
				{
					num++;
				}
				if (i > 0)
				{
					text = text.Substring(0, i - 1) + "-" + text.Substring(num);
				}
				else
				{
					text = "-" + text.Substring(num);
				}
			}
			if (text.Length > 0 && text[text.Length - 1] == '-')
			{
				text += " ";
			}
			this.Push(null, XmlNodeType.Comment, text);
			return true;
		}
		private bool ParseConditionalBlock()
		{
			char c = this.current.ReadChar();
			c = this.current.SkipWhitespace();
			string text = this.current.ScanToken(this.sb, SgmlReader.cdataterm, false);
			if (text != "CDATA")
			{
				this.Log("Expecting CDATA but found '{0}'", new string[]
				{
					text
				});
				this.current.ScanToEnd(null, "CDATA", ">");
				return false;
			}
			c = this.current.SkipWhitespace();
			if (c != '[')
			{
				this.Log("Expecting '[' but found '{0}'", c);
				this.current.ScanToEnd(null, "CDATA", ">");
				return false;
			}
			string value = this.current.ScanToEnd(this.sb, "CDATA", "]]>");
			this.Push(null, XmlNodeType.CDATA, value);
			return true;
		}
		private void ParseDocType()
		{
			char c = this.current.SkipWhitespace();
			string text = this.ScanName(SgmlReader.dtterm);
			this.Push(text, XmlNodeType.DocumentType, null);
			c = this.current.SkipWhitespace();
			if (c != '>')
			{
				string value = "";
				string value2 = "";
				string value3 = "";
				if (c != '[')
				{
					string text2 = this.ScanName(SgmlReader.dtterm);
					if (text2 == "PUBLIC")
					{
						c = this.current.SkipWhitespace();
						if (c == '"' || c == '\'')
						{
							value2 = this.current.ScanLiteral(this.sb, c);
							this.node.AddAttribute(text2, value2, c, this.folding == CaseFolding.None);
						}
					}
					else
					{
						if (text2 != "SYSTEM")
						{
							this.Log("Unexpected token in DOCTYPE '{0}'", new string[]
							{
								text2
							});
							this.current.ScanToEnd(null, "DOCTYPE", ">");
						}
					}
					c = this.current.SkipWhitespace();
					if (c == '"' || c == '\'')
					{
						text2 = this.nametable.Add("SYSTEM");
						value3 = this.current.ScanLiteral(this.sb, c);
						this.node.AddAttribute(text2, value3, c, this.folding == CaseFolding.None);
					}
					c = this.current.SkipWhitespace();
				}
				if (c == '[')
				{
					value = this.current.ScanToEnd(this.sb, "Internal Subset", "]");
					this.node.Value = value;
				}
				c = this.current.SkipWhitespace();
				if (c != '>')
				{
					this.Log("Expecting end of DOCTYPE tag, but found '{0}'", c);
					this.current.ScanToEnd(null, "DOCTYPE", ">");
				}
				if (this.dtd == null)
				{
					this.docType = text;
					this.pubid = value2;
					this.syslit = value3;
					this.subset = value;
					this.LazyLoadDtd(this.current.ResolvedUri);
				}
			}
			this.current.ReadChar();
		}
		private bool ParsePI()
		{
			string array = this.current.ScanToken(this.sb, SgmlReader.piterm, false);
			string value;
			if (this.current.Lastchar != '?')
			{
				value = this.current.ScanToEnd(this.sb, "Processing Instruction", ">");
			}
			else
			{
				value = this.current.ScanToEnd(this.sb, "Processing Instruction", ">");
			}
			if (array != "xml")
			{
				this.Push(this.nametable.Add(array), XmlNodeType.ProcessingInstruction, value);
				return true;
			}
			return false;
		}
		private bool ParseText(char ch, bool newtext)
		{
			bool result = !newtext || this.current.IsWhitespace;
			if (newtext)
			{
				this.sb.Length = 0;
			}
			this.state = State.Text;
			while (ch != '￿')
			{
				if (ch == '<')
				{
					ch = this.current.ReadChar();
					if (ch == '/' || ch == '!' || ch == '?' || char.IsLetter(ch))
					{
						this.state = State.PartialTag;
						this.partial = ch;
						break;
					}
					this.sb.Append('<');
					this.sb.Append(ch);
					result = false;
					ch = this.current.ReadChar();
				}
				else
				{
					if (ch == '&')
					{
						this.ExpandEntity(this.sb, '<');
						result = false;
						ch = this.current.Lastchar;
					}
					else
					{
						if (!this.current.IsWhitespace)
						{
							result = false;
						}
						this.sb.Append(ch);
						ch = this.current.ReadChar();
					}
				}
			}
			string value = this.sb.ToString();
			this.Push(null, XmlNodeType.Text, value);
			return result;
		}
		public string ScanLiteral(StringBuilder sb, char quote)
		{
			sb.Length = 0;
			char c = this.current.ReadChar();
			while (c != '￿' && c != quote)
			{
				if (c == '&')
				{
					this.ExpandEntity(this.sb, quote);
					c = this.current.Lastchar;
				}
				else
				{
					sb.Append(c);
					c = this.current.ReadChar();
				}
			}
			this.current.ReadChar();
			return sb.ToString();
		}
		private bool ParseCData()
		{
			bool flag = this.current.IsWhitespace;
			this.sb.Length = 0;
			char c = this.current.Lastchar;
			if (this.partial != '\0')
			{
				this.Pop();
				char c2 = this.partial;
				switch (c2)
				{
				case ' ':
					break;
				case '!':
					this.partial = ' ';
					return this.ParseComment();
				default:
					if (c2 == '/')
					{
						this.state = State.EndTag;
						return true;
					}
					if (c2 == '?')
					{
						this.partial = ' ';
						return this.ParsePI();
					}
					break;
				}
			}
			else
			{
				c = this.current.ReadChar();
			}
			while (c != '￿')
			{
				if (c == '<')
				{
					c = this.current.ReadChar();
					if (c == '!')
					{
						c = this.current.ReadChar();
						if (c == '-')
						{
							if (flag)
							{
								this.partial = ' ';
								return this.ParseComment();
							}
							this.partial = '!';
							break;
						}
						else
						{
							this.sb.Append('<');
							this.sb.Append('!');
							this.sb.Append(c);
							flag = false;
						}
					}
					else
					{
						if (c == '?')
						{
							this.current.ReadChar();
							if (flag)
							{
								this.partial = ' ';
								return this.ParsePI();
							}
							this.partial = '?';
							break;
						}
						else
						{
							if (c == '/')
							{
								string value = this.sb.ToString();
								if (this.ParseEndTag() && this.endTag == this.node.Name)
								{
									if (flag || value == "")
									{
										return true;
									}
									this.partial = '/';
									this.sb.Length = 0;
									this.sb.Append(value);
									this.state = State.CData;
									break;
								}
								else
								{
									this.sb.Length = 0;
									this.sb.Append(value);
									this.sb.Append("</" + this.endTag + ">");
									flag = false;
								}
							}
							else
							{
								this.sb.Append('<');
								this.sb.Append(c);
								flag = false;
							}
						}
					}
				}
				else
				{
					if (!this.current.IsWhitespace && flag)
					{
						flag = false;
					}
					this.sb.Append(c);
				}
				c = this.current.ReadChar();
			}
			string value2 = this.sb.ToString();
			this.Push(null, XmlNodeType.CDATA, value2);
			if (this.partial == '\0')
			{
				this.partial = ' ';
			}
			return true;
		}
		private void ExpandEntity(StringBuilder sb, char terminator)
		{
			char c = this.current.ReadChar();
			if (c == '#')
			{
				string value = this.current.ExpandCharEntity();
				sb.Append(value);
				c = this.current.ReadChar();
				return;
			}
			this.name.Length = 0;
			while (c != '￿' && (char.IsLetter(c) || c == '_' || c == '-'))
			{
				this.name.Append(c);
				c = this.current.ReadChar();
			}
			string text = this.name.ToString();
			if (this.dtd != null && text != "")
			{
				Entity entity = this.dtd.FindEntity(text);
				if (entity != null)
				{
					if (entity.Internal)
					{
						sb.Append(entity.Literal);
						if (c != terminator)
						{
							c = this.current.ReadChar();
						}
						return;
					}
					Entity entity2 = new Entity(text, entity.PublicId, entity.Uri, this.current.Proxy);
					entity.Open(this.current, new Uri(entity.Uri));
					this.current = entity2;
					this.current.ReadChar();
					return;
				}
				else
				{
					this.Log("Undefined entity '{0}'", new string[]
					{
						text
					});
				}
			}
			sb.Append("&");
			sb.Append(text);
			if (c != terminator)
			{
				sb.Append(c);
				c = this.current.ReadChar();
			}
		}
		public override void Close()
		{
			if (this.current != null)
			{
				this.current.Close();
				this.current = null;
			}
			if (this.log != null)
			{
				this.log.Close();
				this.log = null;
			}
		}
		public override string ReadString()
		{
			if (this.node.NodeType == XmlNodeType.Element)
			{
				this.sb.Length = 0;
				while (this.Read())
				{
					XmlNodeType nodeType = this.NodeType;
					switch (nodeType)
					{
					case XmlNodeType.Text:
					case XmlNodeType.CDATA:
						break;
					default:
						switch (nodeType)
						{
						case XmlNodeType.Whitespace:
						case XmlNodeType.SignificantWhitespace:
							break;
						default:
							return this.sb.ToString();
						}
						break;
					}
					this.sb.Append(this.node.Value);
				}
				return this.sb.ToString();
			}
			return this.node.Value;
		}
		public override string ReadInnerXml()
		{
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.Formatting = Formatting.Indented;
			switch (this.NodeType)
			{
			case XmlNodeType.Element:
				this.Read();
				while (!this.EOF && this.NodeType != XmlNodeType.EndElement)
				{
					xmlTextWriter.WriteNode(this, true);
				}
				this.Read();
				break;
			case XmlNodeType.Attribute:
				stringWriter.Write(this.Value);
				break;
			}
			xmlTextWriter.Close();
			return stringWriter.ToString();
		}
		public override string ReadOuterXml()
		{
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.WriteNode(this, true);
			xmlTextWriter.Close();
			return stringWriter.ToString();
		}
		public override string LookupNamespace(string prefix)
		{
			return null;
		}
		public override void ResolveEntity()
		{
			throw new InvalidOperationException("Not on an entity reference.");
		}
		public override bool ReadAttributeValue()
		{
			if (this.state == State.Attr)
			{
				this.state = State.AttrValue;
				return true;
			}
			if (this.state == State.AttrValue)
			{
				return false;
			}
			throw new InvalidOperationException("Not on an attribute.");
		}
		private void Validate(Node node)
		{
			if (this.dtd != null)
			{
				ElementDecl elementDecl = this.dtd.FindElement(node.Name);
				if (elementDecl != null)
				{
					node.DtdType = elementDecl;
					if (elementDecl.ContentModel.DeclaredContent == DeclaredContent.EMPTY)
					{
						node.IsEmpty = true;
					}
				}
			}
		}
		private void ValidateAttribute(Node node, Attribute a)
		{
			ElementDecl dtdType = node.DtdType;
			if (dtdType != null)
			{
				AttDef attDef = dtdType.FindAttribute(a.Name);
				if (attDef != null)
				{
					a.DtdType = attDef;
				}
			}
		}
		private void ValidateContent(Node node)
		{
			if (this.dtd != null)
			{
				string text = this.nametable.Add(node.Name.ToUpper());
				int i = 0;
				int num = this.stack.Count - 2;
				if (node.DtdType != null)
				{
					for (i = num; i > 0; i--)
					{
						Node node2 = (Node)this.stack[i];
						if (!node2.IsEmpty)
						{
							ElementDecl dtdType = node2.DtdType;
							if (dtdType == null || dtdType.Name == this.dtd.Name || dtdType.CanContain(text, this.dtd) || !dtdType.EndTagOptional)
							{
								break;
							}
						}
					}
				}
				if (i == 0)
				{
					return;
				}
				if (i < num)
				{
					Node node3 = (Node)this.stack[num];
					if (i != num - 1 || !(text == node3.Name))
					{
						string text2 = "";
						for (int j = num; j >= i + 1; j--)
						{
							if (text2 != "")
							{
								text2 += ",";
							}
							Node node4 = (Node)this.stack[j];
							text2 = text2 + "<" + node4.Name + ">";
						}
						this.Log("Element '{0}' not allowed inside '{1}', closing {2}.", new string[]
						{
							text,
							node3.Name,
							text2
						});
					}
					this.state = State.AutoClose;
					this.newnode = node;
					this.Pop();
					this.poptodepth = i + 1;
				}
			}
		}
	}
}
