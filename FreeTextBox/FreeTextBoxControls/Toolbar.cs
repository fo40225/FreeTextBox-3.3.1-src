using FreeTextBoxControls.Support;
using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace FreeTextBoxControls
{
	[DefaultProperty("Items"), ParseChildren(true, "Items"), PersistChildren(true)]
	public class Toolbar : IStateManager
	{
		private bool isTrackingViewState;
		private StateBag viewState;
		private ToolbarItemCollection items;
		private ToolbarButtonStyle buttonStyle;
		[Category("Behavior"), Description("The direction the Toolbar will draw the ToolbarItems"), NotifyParentProperty(true)]
		public RepeatDirection RepeatDirection
		{
			get
			{
				object obj = this.ViewState["RepeatDirection"];
				if (obj != null)
				{
					return (RepeatDirection)this.ViewState["RepeatDirection"];
				}
				return RepeatDirection.Horizontal;
			}
			set
			{
				this.ViewState["RepeatDirection"] = value;
			}
		}
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerDefaultProperty)]
		public ToolbarItemCollection Items
		{
			get
			{
				if (this.items == null)
				{
					this.items = new ToolbarItemCollection();
					if (this.isTrackingViewState)
					{
						((IStateManager)this.items).TrackViewState();
					}
				}
				return this.items;
			}
		}
		[Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual ToolbarButtonStyle ButtonStyle
		{
			get
			{
				if (this.buttonStyle == null)
				{
					this.buttonStyle = new ToolbarButtonStyle();
					if (this.isTrackingViewState)
					{
						((IStateManager)this.buttonStyle).TrackViewState();
					}
				}
				return this.buttonStyle;
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
		public Toolbar()
		{
			this.viewState = new StateBag();
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
			object obj = null;
			object obj2 = null;
			if (savedState != null)
			{
				object[] array = (object[])savedState;
				if (array.Length != 2)
				{
					throw new ArgumentException("Invalid view state");
				}
				obj = array[0];
				obj2 = array[1];
			}
			if (obj != null)
			{
				((IStateManager)this.viewState).LoadViewState(obj);
			}
			if (obj2 != null)
			{
				((IStateManager)this.Items).LoadViewState(obj2);
			}
		}
		object IStateManager.SaveViewState()
		{
			object obj = ((IStateManager)this.viewState).SaveViewState();
			object obj2 = null;
			if (this.items != null)
			{
				obj2 = ((IStateManager)this.items).SaveViewState();
			}
			if (obj != null || obj2 != null)
			{
				return new object[]
				{
					obj,
					obj2
				};
			}
			return null;
		}
		void IStateManager.TrackViewState()
		{
			((IStateManager)this.ViewState).TrackViewState();
			if (this.items != null)
			{
				((IStateManager)this.items).TrackViewState();
			}
		}
	}
}
