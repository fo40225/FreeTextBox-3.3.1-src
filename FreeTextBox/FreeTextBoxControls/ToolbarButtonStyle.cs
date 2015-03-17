using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
namespace FreeTextBoxControls
{
	public class ToolbarButtonStyle : IStateManager
	{
		private bool isTrackingViewState;
		private StateBag viewState;
		[NotifyParentProperty(true)]
		public bool UseBackgroundImage
		{
			get
			{
				object obj = this.ViewState["UseBackgroundImage"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["UseBackgroundImage"] = value;
			}
		}
		[NotifyParentProperty(true)]
		public bool UseOverBackgroundImage
		{
			get
			{
				object obj = this.ViewState["UseOverBackgroundImage"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["UseOverBackgroundImage"] = value;
			}
		}
		[NotifyParentProperty(true)]
		public bool UseDownBackgroundImage
		{
			get
			{
				object obj = this.ViewState["UseDownBackgroundImage"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["UseDownBackgroundImage"] = value;
			}
		}
		[NotifyParentProperty(true)]
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
		[NotifyParentProperty(true)]
		public Color BackColorGradient
		{
			get
			{
				object obj = this.ViewState["BackColorGradient"];
				if (obj != null)
				{
					return (Color)obj;
				}
				return Color.Transparent;
			}
			set
			{
				this.ViewState["BackColorGradient"] = value;
			}
		}
		[NotifyParentProperty(true)]
		public Color BorderColorDark
		{
			get
			{
				object obj = this.ViewState["BorderColorDark"];
				if (obj != null)
				{
					return (Color)obj;
				}
				return Color.Transparent;
			}
			set
			{
				this.ViewState["BorderColorDark"] = value;
			}
		}
		[NotifyParentProperty(true)]
		public Color BorderColorLight
		{
			get
			{
				object obj = this.ViewState["BorderColorLight"];
				if (obj != null)
				{
					return (Color)obj;
				}
				return Color.Transparent;
			}
			set
			{
				this.ViewState["BorderColorLight"] = value;
			}
		}
		[NotifyParentProperty(true)]
		public Color OverBackColor
		{
			get
			{
				object obj = this.ViewState["OverBackColor"];
				if (obj != null)
				{
					return (Color)obj;
				}
				return Color.Transparent;
			}
			set
			{
				this.ViewState["OverBackColor"] = value;
			}
		}
		[NotifyParentProperty(true)]
		public Color OverBackColorGradient
		{
			get
			{
				object obj = this.ViewState["OverBackColorGradient"];
				if (obj != null)
				{
					return (Color)obj;
				}
				return Color.Transparent;
			}
			set
			{
				this.ViewState["OverBackColorGradient"] = value;
			}
		}
		[NotifyParentProperty(true)]
		public Color OverBorderColorLight
		{
			get
			{
				object obj = this.ViewState["OverBorderColorLight"];
				if (obj != null)
				{
					return (Color)obj;
				}
				return Color.Transparent;
			}
			set
			{
				this.ViewState["OverBorderColorLight"] = value;
			}
		}
		[NotifyParentProperty(true)]
		public Color OverBorderColorDark
		{
			get
			{
				object obj = this.ViewState["OverBorderColorDark"];
				if (obj != null)
				{
					return (Color)obj;
				}
				return Color.Transparent;
			}
			set
			{
				this.ViewState["OverBorderColorDark"] = value;
			}
		}
		[NotifyParentProperty(true)]
		public Color DownBackColor
		{
			get
			{
				object obj = this.ViewState["DownBackColor"];
				if (obj != null)
				{
					return (Color)obj;
				}
				return Color.Transparent;
			}
			set
			{
				this.ViewState["DownBackColor"] = value;
			}
		}
		[NotifyParentProperty(true)]
		public Color DownBackColorGradient
		{
			get
			{
				object obj = this.ViewState["DownBackColorGradient"];
				if (obj != null)
				{
					return (Color)obj;
				}
				return Color.Transparent;
			}
			set
			{
				this.ViewState["DownBackColorGradient"] = value;
			}
		}
		[NotifyParentProperty(true)]
		public Color DownBorderColorLight
		{
			get
			{
				object obj = this.ViewState["DownBorderColorLight"];
				if (obj != null)
				{
					return (Color)obj;
				}
				return Color.Transparent;
			}
			set
			{
				this.ViewState["DownBorderColorLight"] = value;
			}
		}
		[NotifyParentProperty(true)]
		public Color DownBorderColorDark
		{
			get
			{
				object obj = this.ViewState["DownBorderColorDark"];
				if (obj != null)
				{
					return (Color)obj;
				}
				return Color.Transparent;
			}
			set
			{
				this.ViewState["DownBorderColorDark"] = value;
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
		public ToolbarButtonStyle()
		{
			this.ViewState["UseBackgroundImage"] = true;
			this.ViewState["BackColor"] = Color.Transparent;
			this.ViewState["BorderColorLight"] = Color.Transparent;
			this.ViewState["BorderColorDark"] = Color.Transparent;
			this.ViewState["OverBackColor"] = Color.Transparent;
			this.ViewState["OverBorderColorLight"] = ColorTranslator.FromHtml("#000080");
			this.ViewState["OverBorderColorDark"] = ColorTranslator.FromHtml("#000080");
			this.ViewState["DownBackColor"] = Color.Transparent;
			this.ViewState["DownBorderColorLight"] = ColorTranslator.FromHtml("#000080");
			this.ViewState["DownBorderColorDark"] = ColorTranslator.FromHtml("#000080");
		}
		public ToolbarButtonStyle(Color backColor, Color borderColorLight, Color borderColorDark, Color overBackColor, Color overBorderColorLight, Color overBorderColorDark, Color downBackColor, Color downBorderColorLight, Color downBorderColorDark)
		{
			this.ViewState["BackColor"] = backColor;
			this.ViewState["BorderColorLight"] = borderColorLight;
			this.ViewState["BorderColorDark"] = borderColorDark;
			this.ViewState["OverBackColor"] = overBackColor;
			this.ViewState["OverBorderColorLight"] = overBorderColorLight;
			this.ViewState["OverBorderColorDark"] = overBorderColorDark;
			this.ViewState["DownBackColor"] = downBackColor;
			this.ViewState["DownBorderColorLight"] = downBorderColorLight;
			this.ViewState["DownBorderColorDark"] = downBorderColorDark;
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
