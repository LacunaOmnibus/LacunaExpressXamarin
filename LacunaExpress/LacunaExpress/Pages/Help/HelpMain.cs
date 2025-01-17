﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

using LacunaExpress.Styles;

namespace LacunaExpress.Pages.Help
{
	public class HelpMain : ContentPage
	{
		public HelpMain()
		{
			//This gets the styles Dictionary and sets it on this page.
			Resources = Styles.Styles.StyleDictionary;

			Label helpOne = new Label () {
				Text = "",
				TextColor = Color.White
			};

			var mainLayout = new StackLayout
			{
				BackgroundColor = Color.FromRgb (0, 0, 128),
				Padding = new Thickness(6, 6, 6, 6),
				Children = {
					helpOne
				}
			};

			Content = mainLayout;
			if (Device.OS == TargetPlatform.iOS)
			{
				mainLayout.Padding = new Thickness (0, 20, 0, 0);
			}
		}
	}
}
