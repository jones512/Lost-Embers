using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Game{

	public class NavigationItem : MonoBehaviour {

		public Transform pivotTransform;
		public enum Navigation { None, Horizontal, Vertical, Grid};
		public Navigation navigation { get { return _navigation; } }		
		public int navigationIndex { set; get; } //id index for this item used as counter for gamepad navigation
        public int navigationItemId { set; get; } //id item set on start
        public int totalNavigationPoints { set; get; }
        public Slider slider { set; get; } 
        public GridLayoutGroup grid { set; get; }


		public NavigationItem itemUp;
		public NavigationItem itemDown;
		public NavigationItem itemLeft;
		public NavigationItem itemRight;

		private Navigation _navigation = Navigation.None;
        public System.Action<int> EventOnClick; //pass the navigationIndex

		void Awake(){
            slider = GetComponent<Slider>();
            grid = GetComponent<GridLayoutGroup>();

			if (pivotTransform == null){
				pivotTransform = this.transform;
			}

			UpdateObjectStats ();

            Button but = this.GetComponent<Button>();

            if (but != null) {
                but.onClick.AddListener(OnClick);
            }
		}

        void OnDestroy() {
            Button but = this.GetComponent<Button>();

            if (but != null) {
                but.onClick.RemoveListener(OnClick);
            }
        }


        void OnClick() {       
            if (EventOnClick != null)
                EventOnClick(navigationItemId);
        }

        //assume you've added a new item to our navigation list
        //a grid could have new objects added and we need to update the totalcounter
        public void UpdateObjectStats(){
            if (slider != null) {

                if (slider.handleRect != null)
                    pivotTransform = slider.handleRect.transform;

                if (slider.direction == Slider.Direction.RightToLeft || slider.direction == Slider.Direction.LeftToRight) {
                    _navigation = NavigationItem.Navigation.Horizontal;
                }else {  _navigation = NavigationItem.Navigation.Vertical; }

                totalNavigationPoints = (int)slider.maxValue;

                return;
            }

            if (grid != null) {
                _navigation = NavigationItem.Navigation.Grid;
                totalNavigationPoints = grid.transform.childCount;// .GetComponentsInChildren<RectTransform>().Length;
            }
		}

	}
}