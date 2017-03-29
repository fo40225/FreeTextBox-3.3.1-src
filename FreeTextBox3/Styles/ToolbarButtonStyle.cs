using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Resources;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Drawing;
using FreeTextBoxControls.Support;

namespace FreeTextBoxControls {	
	/// <summary>
	/// Contains all the style parameters for ToolbarButtons
	/// </summary>
	public class ToolbarButtonStyle : IStateManager {
		
		#region Constuctors
		/// <summary>
		/// Default properties (Office 2003)
		/// </summary>
		public ToolbarButtonStyle() {
			ViewState["UseBackgroundImage"] = true;
			
			ViewState["BackColor"] = Color.Transparent;
			ViewState["BorderColorLight"] = Color.Transparent;
			ViewState["BorderColorDark"] = Color.Transparent;
			
			ViewState["OverBackColor"] = Color.Transparent;
			ViewState["OverBorderColorLight"] = ColorTranslator.FromHtml("#000080");
			ViewState["OverBorderColorDark"] = ColorTranslator.FromHtml("#000080");
			
			ViewState["DownBackColor"] = Color.Transparent;
			ViewState["DownBorderColorLight"] = ColorTranslator.FromHtml("#000080");
			ViewState["DownBorderColorDark"] = ColorTranslator.FromHtml("#000080");
		}
		
		
		/// <summary>
		/// Constructor for colors only
		/// </summary>
		public ToolbarButtonStyle(Color backColor, Color borderColorLight,Color borderColorDark,Color overBackColor,Color overBorderColorLight,Color overBorderColorDark,Color downBackColor,Color downBorderColorLight,Color downBorderColorDark) {
			
			ViewState["BackColor"] = backColor;
			ViewState["BorderColorLight"] = borderColorLight;
			ViewState["BorderColorDark"] = borderColorDark;
			
			ViewState["OverBackColor"] = overBackColor;
			ViewState["OverBorderColorLight"] = overBorderColorLight;
			ViewState["OverBorderColorDark"] = overBorderColorDark;
			
			ViewState["DownBackColor"] = downBackColor;
			ViewState["DownBorderColorLight"] = downBorderColorLight;
			ViewState["DownBorderColorDark"] = downBorderColorDark;
		}
		#endregion

		#region Public Properties

		/// <summary>
		/// The background image in the normal state.
		/// </summary>	
		[
		NotifyParentProperty(true)
		]
		public bool UseBackgroundImage {
			get { 				
				object savedState = this.ViewState["UseBackgroundImage"];
				return (savedState == null) ? false : (bool) savedState;
			}
			set { ViewState["UseBackgroundImage"] = value;}		
		}
		/// <summary>
		/// The background image onMouseOver.
		/// </summary>	
		[
		NotifyParentProperty(true)
		]
		public bool UseOverBackgroundImage {
			get { 				
				object savedState = this.ViewState["UseOverBackgroundImage"];
				return (savedState == null) ? false : (bool) savedState;
			}
			set { ViewState["UseOverBackgroundImage"] = value;}		
		}
		/// <summary>
		/// The background image onMouseDown (UNUSED).
		/// </summary>
		[
		NotifyParentProperty(true)
		]
		public bool UseDownBackgroundImage {
			get { 				
				object savedState = this.ViewState["UseDownBackgroundImage"];
				return (savedState == null) ? false : (bool) savedState;
			}
			set { ViewState["UseDownBackgroundImage"] = value;}
		}

		#region Normal
		/// <summary>
		/// The default back color of buttons.
		/// </summary>	
		[
		NotifyParentProperty(true)
		]
		public Color BackColor {
			get { 				
				object savedState = this.ViewState["BackColor"];
				return (savedState == null) ? Color.Transparent : (Color) savedState;
			}
			set { ViewState["BackColor"] = value;}
		}
		/// <summary>
		/// IE only property to use gradients.
		/// </summary>	
		[
		NotifyParentProperty(true)
		]
		public Color BackColorGradient {
			get { 				
				object savedState = this.ViewState["BackColorGradient"];
				return (savedState == null) ? Color.Transparent : (Color) savedState;
			}
			set { ViewState["BackColorGradient"] = value;}
		}
		/// <summary>
		/// The default dark (right and bottom) border color of buttons.
		/// </summary>		
		[
		NotifyParentProperty(true)
		]
		public Color BorderColorDark {
			get { 				
				object savedState = this.ViewState["BorderColorDark"];
				return (savedState == null) ? Color.Transparent : (Color) savedState;
			}
			set { ViewState["BorderColorDark"] = value;}
		}
		/// <summary>
		/// The default light (top and left) border color of buttons.
		/// </summary>	
		[
		NotifyParentProperty(true)
		]
		public Color BorderColorLight {
			get { 				
				object savedState = this.ViewState["BorderColorLight"];
				return (savedState == null) ? Color.Transparent : (Color) savedState;
			}
			set { ViewState["BorderColorLight"] = value;}
		}
		#endregion
		
		#region Over
		/// <summary>
		/// The default onMouseOver back color of buttons.
		/// </summary>	
		[
		NotifyParentProperty(true)
		]
		public Color OverBackColor {
			get { 				
				object savedState = this.ViewState["OverBackColor"];
				return (savedState == null) ? Color.Transparent : (Color) savedState;
			}
			set { ViewState["OverBackColor"] = value;}
		}
		/// <summary>
		/// IE only property to use gradients.
		/// </summary>	
		[
		NotifyParentProperty(true)
		]
		public Color OverBackColorGradient {
			get { 				
				object savedState = this.ViewState["OverBackColorGradient"];
				return (savedState == null) ? Color.Transparent : (Color) savedState;
			}
			set { ViewState["OverBackColorGradient"] = value;}
		}
		/// <summary>
		/// The default onMouseOver light (top and left) border color of buttons.
		/// </summary>	
		[
		NotifyParentProperty(true)
		]
		public Color OverBorderColorLight {
			get { 				
				object savedState = this.ViewState["OverBorderColorLight"];
				return (savedState == null) ? Color.Transparent : (Color) savedState;
			}
			set { ViewState["OverBorderColorLight"] = value;}
		}
		/// <summary>
		/// The default onMouseOver dark (right and bottom) border color of buttons.
		/// </summary>		
		[
		NotifyParentProperty(true)
		]
		public Color OverBorderColorDark {
			get { 				
				object savedState = this.ViewState["OverBorderColorDark"];
				return (savedState == null) ? Color.Transparent : (Color) savedState;
			}
			set { ViewState["OverBorderColorDark"] = value;}
		}
		#endregion

		#region Down
		/// <summary>
		/// The default onMouseDown back color of buttons.
		/// </summary>	
		[
		NotifyParentProperty(true)
		]
		public Color DownBackColor {
			get { 				
				object savedState = this.ViewState["DownBackColor"];
				return (savedState == null) ? Color.Transparent : (Color) savedState;
			}
			set { ViewState["DownBackColor"] = value;}
		}
		/// <summary>
		/// IE only property to use gradients.
		/// </summary>	
		[
		NotifyParentProperty(true)
		]
		public Color DownBackColorGradient {
			get { 				
				object savedState = this.ViewState["DownBackColorGradient"];
				return (savedState == null) ? Color.Transparent : (Color) savedState;
			}
			set { ViewState["DownBackColorGradient"] = value;}
		}
		/// <summary>
		/// The default onMouseDown light (top and left) border color of buttons onMouseDown.
		/// </summary>	
		[
		NotifyParentProperty(true)
		]
		public Color DownBorderColorLight {
			get { 				
				object savedState = this.ViewState["DownBorderColorLight"];
				return (savedState == null) ? Color.Transparent : (Color) savedState;
			}
			set { ViewState["DownBorderColorLight"] = value;}
		}
		/// <summary>
		/// The default onMouseDown dark (right and bottom) border color of buttons onMouseDown.
		/// </summary>	
		[
		NotifyParentProperty(true)
		]
		public Color DownBorderColorDark {
			get { 				
				object savedState = this.ViewState["DownBorderColorDark"];
				return (savedState == null) ? Color.Transparent : (Color) savedState;
			}
			set { ViewState["DownBorderColorDark"] = value;}
		}
		#endregion

		#endregion

		#region Private Properties
		private bool isTrackingViewState;
		private StateBag viewState;
		#endregion
		
		#region ViewState
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		protected StateBag ViewState {
			get {
				if (viewState == null) {
					viewState = new StateBag(false);
					if (isTrackingViewState) {
						((IStateManager)viewState).TrackViewState();
					}
				}
				return viewState;
			}
		}
		internal void SetDirty() {
			if (viewState != null) {
				ICollection Keys = viewState.Keys;
				foreach (string key in Keys) {
					viewState.SetItemDirty(key, true);
				}
			}
		}
		#endregion
		
		#region IStateManger implemenation
		bool IStateManager.IsTrackingViewState {
			get {
				return isTrackingViewState;
			}
		}

		void IStateManager.LoadViewState(object savedState) {
			if (savedState != null) {
				((IStateManager)viewState).LoadViewState(savedState);
			}
		}

		object IStateManager.SaveViewState() {
			if (viewState != null) {
				return ((IStateManager)viewState).SaveViewState();
			}
			return null;
		}
		
		void IStateManager.TrackViewState() {
			
			isTrackingViewState = true;

			if (viewState != null) {
				((IStateManager)viewState).TrackViewState();
			}
		}
		#endregion
	}	
}