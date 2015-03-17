using FreeTextBoxControls.Support;
using System;
using System.ComponentModel;
using System.Web.UI;
namespace FreeTextBoxControls
{
	public class ToolbarDropDownList : ToolbarItem, IParserAccessor, IStateManager
	{
		private ToolbarDropDownListItemCollection items;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerDefaultProperty)]
		public ToolbarDropDownListItemCollection Items
		{
			get
			{
				if (this.items == null)
				{
					this.items = new ToolbarDropDownListItemCollection();
					if (this.g_isTrackingViewState)
					{
						((IStateManager)this.items).TrackViewState();
					}
				}
				return this.items;
			}
		}
		public ToolbarDropDownList() : this("", "")
		{
		}
		public ToolbarDropDownList(string title)
		{
			base.SetTitleLanguage(title);
		}
		public ToolbarDropDownList(string title, string nothing)
		{
			base.SetTitleLanguage(title);
		}
		void IStateManager.TrackViewState()
		{
			((IStateManager)this.g_viewState).TrackViewState();
			if (this.items != null)
			{
				((IStateManager)this.items).TrackViewState();
			}
		}
		object IStateManager.SaveViewState()
		{
			object[] array = new object[2];
			array[0] = ((IStateManager)this.g_viewState).SaveViewState();
			if (this.items != null)
			{
				array[1] = ((IStateManager)this.items).SaveViewState();
			}
			return array;
		}
		void IStateManager.LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				object[] array = (object[])savedState;
				((IStateManager)this.g_viewState).LoadViewState(array[0]);
				if (array[1] != null)
				{
					((IStateManager)this.Items).LoadViewState(array[1]);
				}
			}
		}
		public void AddParsedSubObject(object obj)
		{
			if (obj is ToolbarListItem)
			{
				this.Items.Add((ToolbarListItem)obj);
			}
		}
	}
}
