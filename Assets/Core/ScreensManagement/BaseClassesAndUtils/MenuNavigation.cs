using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Game{

	[System.Serializable]
	public class MenuNavigation {
			
		public NavigationItem[] navigationItems;
		//pivot that moves to select a new item, aka called "it moves to the new selected position"
		public Transform itemSelectionTR;

        public System.Action<int> EventOnClick;

		private int _indexSelected = 0;
        private int _indexSubItemSelected = 0;
        private Vector3 pressed = new Vector3(1.2f, 1.2f, 1.2f);
        private Vector3 mScale = Vector3.one;

		#region init, destroy
		public void Init(){
			UpdateWithNewItems ();

            if (itemSelectionTR != null)
                mScale = itemSelectionTR.localScale;
		}

		public void Destroy(){
            for (int i = 0; i < navigationItems.Length; i++) {
                NavigationItem item = navigationItems[i];
                item.EventOnClick -= OnClickItemCallback;
            }

            System.Array.Clear (navigationItems, 0, navigationItems.Length);
			navigationItems = null;
		}
		#endregion


        #region public calls
        public int GetIndexSelected(){
			return _indexSelected;
		}

        public int GetIndexSubItemSelected() {
            return _indexSubItemSelected;
        }

		public void UpdateWithNewItems(){
			for (int i = 0; i < navigationItems.Length; i++) {
				NavigationItem item = navigationItems [i];
                item.EventOnClick += OnClickItemCallback;
                item.navigationItemId = i;
				item.UpdateObjectStats ();
			}
		}


        public void ShowSelectionEffect() {
            LeanTween.scale(itemSelectionTR.gameObject, pressed, 0.1f).setEase(LeanTweenType.linear).setOnComplete(RestoreScale);
        }

        void OnClickItemCallback(int idClick) {
            _indexSelected = idClick;
            MoveSelectionItem();

            if (EventOnClick != null)
                EventOnClick(_indexSelected);
        }
        #endregion


        #region menu navigation
        public void SelectItem(int index) {
            _indexSelected = index;
            MoveSelectionItem();
        }

        public void SelectFirstItem() {
            _indexSelected = 0;
            _indexSubItemSelected = 0;
            NavigationItem item = navigationItems[0];
            GridLayoutGroup grid = item.grid;
            if (grid != null) {
                if (grid.transform.GetChild(0) != null) {
                    itemSelectionTR.position = grid.transform.GetChild(0).position;
                }
            } else {
                MoveSelectionItem();
            }
        }

		public void SelectOption(GamepadController.directionGamepadEnum directionGamepad)
		{
			switch (directionGamepad)
			{
			case GamepadController.directionGamepadEnum.Down:

				if (_indexSelected == navigationItems.Length - 1) {
					_indexSelected = 0;
				} else {
					_indexSelected += 1;
				}
				break;

			case GamepadController.directionGamepadEnum.Up:

				if (_indexSelected == 0) {
					_indexSelected = navigationItems.Length - 1;
				} else {
					_indexSelected -= 1;
				}
				break;

			case GamepadController.directionGamepadEnum.Left:
				if (_indexSelected == 0) {
					_indexSelected = navigationItems.Length - 1;
				} else {
					_indexSelected -= 1;
				}
				break;

			case GamepadController.directionGamepadEnum.Right:
				if (_indexSelected == navigationItems.Length - 1) {
					_indexSelected = 0;
				} else {
					_indexSelected += 1;
				}
				break;

			default:
				break;
			}

			MoveSelectionItem();
		}
	
		public void SelectOptionWithItems(GamepadController.directionGamepadEnum directionGamepad)
		{
			NavigationItem item = navigationItems [_indexSelected];

			switch (directionGamepad)
			{
			case GamepadController.directionGamepadEnum.Down:

				if (item.navigation == NavigationItem.Navigation.Vertical) {

					Slider slider = item.slider;

					item.navigationIndex++;

					if (item.navigationIndex > slider.maxValue)
						item.navigationIndex = (int)slider.maxValue;

					slider.value = ((float)item.navigationIndex / (float)item.totalNavigationPoints);

				} else {
					if (item.itemDown != null) {
						_indexSelected = item.itemDown.navigationItemId;
					} 
				}
				break;

			case GamepadController.directionGamepadEnum.Up:
				if (item.navigation == NavigationItem.Navigation.Vertical) {

					item.navigationIndex--;

					Slider slider = item.slider;

					if (item.navigationIndex < 0) {

						item.navigationIndex = 0;
						slider.value = 0f;
					} else {
						slider.value = ((float)item.navigationIndex / (float)item.totalNavigationPoints);
					}

				} else {
					if (item.itemUp != null) {
						_indexSelected = item.itemUp.navigationItemId;
					} 
				}
				break;

			case GamepadController.directionGamepadEnum.Left:
				if (item.navigation == NavigationItem.Navigation.Horizontal) {
					Slider slider = item.slider;

					item.navigationIndex--;

					if (item.navigationIndex < 0) {
						
						item.navigationIndex = 0;
						slider.value = 0f;
					} else {
						slider.value = ((float)item.navigationIndex / (float)item.totalNavigationPoints);
					}

				} else {
					if (item.itemLeft != null) {
						_indexSelected = item.itemLeft.navigationItemId;
					}
				}
				break;

			case GamepadController.directionGamepadEnum.Right:

				if (item.navigation == NavigationItem.Navigation.Horizontal) {
					Slider slider = item.slider;

					item.navigationIndex++;

					if (item.navigationIndex > slider.maxValue)
						item.navigationIndex = (int)slider.maxValue;

					slider.value = ((float)item.navigationIndex / (float)item.totalNavigationPoints);
				} else {
					if (item.itemRight != null) {
						_indexSelected = item.itemRight.navigationItemId;
					} 
				}
				break;

			default:
				break;
			}
				
			MoveSelectionItem ();
		}

		public void SelectOptionInGrid(int maxItemsPerLine, GamepadController.directionGamepadEnum directionGamepad)
		{
			switch (directionGamepad)
			{
			case GamepadController.directionGamepadEnum.Down:
				_indexSelected += maxItemsPerLine;

				if (_indexSelected > (navigationItems.Length - 1)) {
					int newIndex = 0;
					newIndex = (_indexSelected - maxItemsPerLine);

                    _indexSelected -= newIndex; 
				}
				break;

			case GamepadController.directionGamepadEnum.Up:
				_indexSelected -= maxItemsPerLine;

				if (_indexSelected < 0) {
					int newIndexUp = 0;
					newIndexUp = (_indexSelected + maxItemsPerLine);

					_indexSelected = (navigationItems.Length) - maxItemsPerLine + newIndexUp;
				}
				break;

			case GamepadController.directionGamepadEnum.Left:
				if (_indexSelected == 0) {
					_indexSelected = (navigationItems.Length - 1);
				} else {
					_indexSelected -= 1;
				}
				break;

			case GamepadController.directionGamepadEnum.Right:
				if (_indexSelected == (navigationItems.Length - 1)) {
					_indexSelected = 0;
				} else {
					_indexSelected += 1;
				}
				break;

			default:
				break;
			}

			MoveSelectionItem();
		}
			
		public void SelectOptionGridwithItems(GamepadController.directionGamepadEnum directionGamepad)
		{
			NavigationItem item = navigationItems [_indexSelected];

			switch (directionGamepad)
			{
			case GamepadController.directionGamepadEnum.Down:
				if (item.navigation == NavigationItem.Navigation.Vertical) {

					Slider slider = item.slider;

					item.navigationIndex++;

					if (item.navigationIndex > slider.maxValue)
						item.navigationIndex = (int)slider.maxValue;

					slider.value = ((float)item.navigationIndex / (float)item.totalNavigationPoints);

                    _indexSubItemSelected = item.navigationIndex;

					MoveSelectionItem();

				} else if (item.navigation == NavigationItem.Navigation.Grid) {
					GridLayoutGroup grid = item.grid;

                    int oldIndex = item.navigationIndex;

					item.navigationIndex += grid.constraintCount;
		
					if (item.navigationIndex > item.totalNavigationPoints) {
						if (item.itemDown != null) {
							_indexSelected = item.itemDown.navigationItemId;
							MoveSelectionItem ();
						} else {
                            if (item.totalNavigationPoints < grid.constraintCount) {
                                item.navigationIndex = oldIndex; //stay where you are...
                            } else {
                                //we have let's say 2 rows with 6 items per row
                                //from 0-5 first line
                                //6-11 second line
                                //now we get the negative number that came up by substracting the maxperline
                                //and we turn it into a positive number
                                //
                                //now check how many lines in total we have, could be a grid of 2, 3, 4, etc lines
                                //round the number to get positive number of line (no 1.5 lines)
                                //if more than 2 lines, then multiply the maxperline by the number of totallines
                                //this will give you the max per line, for example in a 2 lines it will tell you that you have 12 items
                                //but we want to start at the beginning of the line, so we substract the maxperline to give us the init point


                                //imagine a grid like this
                                // 0 1 2 3 4  5 
                                // 6 7 8 9 10 11
                                //we want to move up from 3 and we should go to number 9
                                //and in case of less items than 9, then we move to our maxitems number

                                int totalLines = (int)(((float)item.totalNavigationPoints / (float)grid.constraintCount) + 0.5f);

                                if (totalLines > 2) {
                                    if (oldIndex >= ((grid.constraintCount * totalLines) - grid.constraintCount)) {
                                        item.navigationIndex = oldIndex - ((grid.constraintCount * totalLines) - grid.constraintCount);
                                    } else {
                                        item.navigationIndex = oldIndex + ((grid.constraintCount * totalLines) - grid.constraintCount);
                                    }
                                } else {
                                    if (oldIndex < grid.constraintCount) {
                                        item.navigationIndex = oldIndex + grid.constraintCount;
                                    } else {
                                        item.navigationIndex = oldIndex - grid.constraintCount;
                                    }
                                }


                                if (item.navigationIndex >= item.totalNavigationPoints)
                                    item.navigationIndex = item.totalNavigationPoints - 1;

                                _indexSubItemSelected = item.navigationIndex;
                                MoveSelectionItem(grid.transform.GetChild(item.navigationIndex));
                            }
						}
					} else {
                        if (item.navigationIndex >= item.totalNavigationPoints)
                            item.navigationIndex = item.totalNavigationPoints - 1;

                        _indexSubItemSelected = item.navigationIndex;
						MoveSelectionItem (grid.transform.GetChild (item.navigationIndex));
					}
				}
				else {
					SelectOption (directionGamepad);
				}
				break;

			case GamepadController.directionGamepadEnum.Up:
				if (item.navigation == NavigationItem.Navigation.Vertical) {

					item.navigationIndex--;

					Slider slider = item.slider;

					if (item.navigationIndex <= 0) {

						item.navigationIndex = 0;
						slider.value = 0f;
					} else {
						slider.value = ((float)item.navigationIndex / (float)item.totalNavigationPoints);
					}

                    _indexSubItemSelected = item.navigationIndex;

					MoveSelectionItem();

				} else if (item.navigation == NavigationItem.Navigation.Grid) {
					GridLayoutGroup grid = item.grid;

                    int oldIndex = item.navigationIndex;

					item.navigationIndex -= grid.constraintCount;

					if (item.navigationIndex <= 0) {
						if (item.itemUp != null) {
							_indexSelected = item.itemUp.navigationItemId;
							MoveSelectionItem ();
						} else {
                            if (item.totalNavigationPoints < grid.constraintCount) {
                                item.navigationIndex = oldIndex; //stay where you are...
                            } else {
                                if (item.navigationIndex == 0) {
                                    _indexSubItemSelected = item.navigationIndex;
                                    MoveSelectionItem(grid.transform.GetChild(item.navigationIndex));
                                } else {

                                    //we have let's say 2 rows with 6 items per row
                                    //from 0-5 first line
                                    //6-11 second line
                                    //now we get the negative number that came up by substracting the maxperline
                                    //and we turn it into a positive number
                                    //
                                    //now check how many lines in total we have, could be a grid of 2, 3, 4, etc lines
                                    //round the number to get positive number of line (no 1.5 lines)
                                    //if more than 2 lines, then multiply the maxperline by the number of totallines
                                    //this will give you the max per line, for example in a 2 lines it will tell you that you have 12 items
                                    //but we want to start at the beginning of the line, so we substract the maxperline to give us the init point


                                    //imagine a grid like this
                                    // 0 1 2 3 4  5 
                                    // 6 7 8 9 10 11
                                    //we want to move up from 3 and we should go to number 9
                                    //and in case of less items than 9, then we move to our maxitems number

                                    //calculate how many lines in our grid, round the value to get a multiple number
                                    int totalLines = (int)(((float)item.totalNavigationPoints / (float)grid.constraintCount) + 0.5f);

                                    if (totalLines > 2) {
                                        item.navigationIndex = oldIndex + ((grid.constraintCount * totalLines) - grid.constraintCount);
                                    } else {
                                        item.navigationIndex = oldIndex + grid.constraintCount;
                                    }

                                    if (item.navigationIndex >= item.totalNavigationPoints)
                                        item.navigationIndex = item.totalNavigationPoints-1;
                          

                                    _indexSubItemSelected = item.navigationIndex;
                                    MoveSelectionItem(grid.transform.GetChild(item.navigationIndex));
                                }
                            }
						}
					} else {
                        _indexSubItemSelected = item.navigationIndex;
						MoveSelectionItem (grid.transform.GetChild (item.navigationIndex));
					}
				}
				else {
					SelectOption (directionGamepad);
				} 

				break;

			case GamepadController.directionGamepadEnum.Left:
				if (item.navigation == NavigationItem.Navigation.Horizontal) {
					Slider slider = item.slider;

					item.navigationIndex--;

					if (item.navigationIndex <= 0) {

						item.navigationIndex = 0;
						slider.value = 0f;
					} else {
						slider.value = ((float)item.navigationIndex / (float)item.totalNavigationPoints);
					}

                    _indexSubItemSelected = item.navigationIndex;

					MoveSelectionItem();

				} else if (item.navigation == NavigationItem.Navigation.Grid) {
					GridLayoutGroup grid = item.grid;

                    item.navigationIndex--;

					if (item.navigationIndex < 0) {
						if (item.itemLeft != null) {
							_indexSelected = item.itemLeft.navigationItemId;
							MoveSelectionItem ();
						} else {
							item.navigationIndex = (item.totalNavigationPoints - 1);
                            _indexSubItemSelected = item.navigationIndex;
							MoveSelectionItem(grid.transform.GetChild(item.navigationIndex));
						}
					} else {
                        _indexSubItemSelected = item.navigationIndex;
						MoveSelectionItem(grid.transform.GetChild(item.navigationIndex));
					}
				}
				else {
					SelectOption (directionGamepad);
				}  
				break;

			case GamepadController.directionGamepadEnum.Right:
				if (item.navigation == NavigationItem.Navigation.Horizontal) {
					Slider slider = item.slider;

					item.navigationIndex++;

					if (item.navigationIndex > slider.maxValue)
						item.navigationIndex = (int)slider.maxValue;

					slider.value = ((float)item.navigationIndex / (float)item.totalNavigationPoints);

                    _indexSubItemSelected = item.navigationIndex;

					MoveSelectionItem();

				} else if (item.navigation == NavigationItem.Navigation.Grid) {
					GridLayoutGroup grid = item.grid;

                    item.navigationIndex++;

					if (item.navigationIndex > (item.totalNavigationPoints - 1)) {
						if (item.itemRight != null) {
							_indexSelected = item.itemRight.navigationItemId;
							MoveSelectionItem();
						} else {
							item.navigationIndex = 0;
                            _indexSubItemSelected = item.navigationIndex;
							MoveSelectionItem(grid.transform.GetChild(item.navigationIndex));
						}
					} else {
                        _indexSubItemSelected = item.navigationIndex;
                        MoveSelectionItem(grid.transform.GetChild(item.navigationIndex));
					}
				}
				else {
					SelectOption (directionGamepad);
				}   
				break;

			default:
				break;
			}
		}

        

		void MoveSelectionItem()
		{
            if (itemSelectionTR != null) {
                itemSelectionTR.SetParent(navigationItems[_indexSelected].pivotTransform, false);
                itemSelectionTR.SetAsFirstSibling();
                itemSelectionTR.position = navigationItems[_indexSelected].pivotTransform.position;
                LeanTween.scale(itemSelectionTR.gameObject, pressed, 0.1f).setEase(LeanTweenType.linear).setOnComplete(RestoreScale);
            }
        }

		void MoveSelectionItem(Transform aTransform)
		{
            if (itemSelectionTR != null) {
                itemSelectionTR.SetParent(aTransform, false);
                itemSelectionTR.SetAsFirstSibling();
                itemSelectionTR.position = aTransform.position;

                LeanTween.scale(itemSelectionTR.gameObject, pressed, 0.1f).setEase(LeanTweenType.linear).setOnComplete(RestoreScale);
            }
		}

        void RestoreScale() {
            if (itemSelectionTR != null) {
                itemSelectionTR.localScale = mScale;
            }
        }
		#endregion


       
	}
}
