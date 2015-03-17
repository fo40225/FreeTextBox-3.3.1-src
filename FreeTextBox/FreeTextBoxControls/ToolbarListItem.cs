using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
namespace FreeTextBoxControls
{
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class ToolbarListItem : IStateManager, IParserAccessor
	{
		private bool isTrackingViewState;
		private StateBag viewState;
		[Category("Behavior"), Description("The value used in the JavaScript function"), NotifyParentProperty(true)]
		public string Value
		{
			get
			{
				object obj = this.ViewState["Value"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "";
			}
			set
			{
				this.ViewState["Value"] = value;
			}
		}
		[Category("Behavior"), Description("The background color of the dropdownlistitem"), NotifyParentProperty(true)]
		public Color BackColor
		{
			get
			{
				object obj = this.ViewState["BackColor"];
				if (obj != null)
				{
					return (Color)obj;
				}
				return Color.Transparent;
			}
			set
			{
				this.ViewState["BackColor"] = value;
			}
		}
		[Category("Behavior"), DefaultValue(""), Description("The display text for the item"), NotifyParentProperty(true), PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
		public string Text
		{
			get
			{
				object obj = this.ViewState["Text"];
				if (obj != null)
				{
					return (string)obj;
				}
				return "";
			}
			set
			{
				this.ViewState["Text"] = value;
			}
		}
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected StateBag ViewState
		{
			get
			{
				if (this.viewState == null)
				{
					this.viewState = new StateBag(false);
					if (this.isTrackingViewState)
					{
						((IStateManager)this.viewState).TrackViewState();
					}
				}
				return this.viewState;
			}
		}
		bool IStateManager.IsTrackingViewState
		{
			get
			{
				return this.isTrackingViewState;
			}
		}
		public ToolbarListItem() : this("")
		{
		}
		public ToolbarListItem(string text)
		{
			this.Text = text;
			this.Value = text;
		}
		public ToolbarListItem(string text, string value)
		{
			this.Text = text;
			this.Value = value;
		}
		public ToolbarListItem(string text, string value, Color color)
		{
			this.Text = text;
			this.Value = value;
			this.BackColor = color;
		}
		public ToolbarListItem(string text, Color color)
		{
			this.Text = text;
			this.Value = text;
			this.BackColor = color;
		}
		void IParserAccessor.AddParsedSubObject(object obj)
		{
			if (obj is LiteralControl)
			{
				this.Text = ((LiteralControl)obj).Text;
			}
		}
		internal void SetDirty()
		{
			if (this.viewState != null)
			{
				ICollection keys = this.viewState.Keys;
				foreach (string key in keys)
				{
					this.viewState.SetItemDirty(key, true);
				}
			}
		}
		void IStateManager.LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				((IStateManager)this.viewState).LoadViewState(savedState);
			}
		}
		object IStateManager.SaveViewState()
		{
			if (this.viewState != null)
			{
				return ((IStateManager)this.viewState).SaveViewState();
			}
			return null;
		}
		void IStateManager.TrackViewState()
		{
			this.isTrackingViewState = true;
			if (this.viewState != null)
			{
				((IStateManager)this.viewState).TrackViewState();
			}
		}
	}
}
