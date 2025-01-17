﻿using LacunaExpress.AccountManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;
using LacunaExpanseAPIWrapper;

using LacunaExpress.Styles;

namespace LacunaExpress.Pages.CaptchaPage
{

	public class CaptchaPage : ContentPage
	{
		// Just specified the Styles.Styles.StyleDictionary in the code instead for each instance.
		// Couldn't figure out error.
		//This gets the styles Dictionary and sets it on this page.
		

		string guid;
		Image captchaImage = new Image {
			VerticalOptions = LayoutOptions.Center,
			Aspect = Aspect.Fill,
			WidthRequest = 50, 
			HeightRequest = 80
		};

		public CaptchaPage(AccountModel account)
		{
            Resources = Styles.Styles.StyleDictionary;
			BoxView topSpacer = new BoxView
			{
				BackgroundColor = Color.Transparent,
				VerticalOptions = LayoutOptions.StartAndExpand,
			};
            Entry answerEntry = new Entry
            {
                Placeholder = "Answer",
				Style = (Style)Styles.Styles.StyleDictionary[Styles.Styles.StyleName.EntryStyle.ToString()],				
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};
            Button answerButton = new Button
            {
                Text = "Answer",
				Style = (Style)Styles.Styles.StyleDictionary[Styles.Styles.StyleName.RegularButton.ToString()],
				VerticalOptions = LayoutOptions.EndAndExpand,
            };
			var mainLayout = new StackLayout
			{
				Style = (Style)Styles.Styles.StyleDictionary[Styles.Styles.StyleName.MainLayout.ToString()],
				Children = {
					topSpacer,
					captchaImage,
					answerEntry,
					answerButton
				}
			};

			Content = mainLayout;
			if (Device.OS == TargetPlatform.iOS)
			{
				mainLayout.Padding = new Thickness (0, 20, 0, 0);
			}

			answerButton.Clicked += async (sender, e) =>{
				if (answerEntry.Text.Length > 0)
				{
					answerButton.IsEnabled = false;
					var s = new LacunaExpress.Data.Server();
					var json = Captcha.Solve(account.SessionID, guid, answerEntry.Text);
					var response = await s.GetHttpResultStringAsyncAsString(account.Server, Captcha.url, json);
					if (response != null)
					{
						var r = Newtonsoft.Json.JsonConvert.DeserializeObject<CaptchaResponse>(response);
						if(r.result==1){
							var oldacnt = account;
							account.CaptchaLastRenewed = DateTime.Now;
							AccountManager accountMan = new AccountManager();
							accountMan.ModifyAccountAsync(account, oldacnt);
							await Navigation.PopModalAsync();
						}
						else
						{
							answerButton.IsEnabled = true;
							GetCaptcha(account);
							answerEntry.Text = "";
						}
					}
					else
					{
						answerButton.IsEnabled = true;
						GetCaptcha(account);
						answerEntry.Text = "";
					}
				}
			};
			this.Appearing += async (sender, e) =>
			{
				var captchaValidUntil = DateTime.Now.AddMinutes(-25);
				if (account.CaptchaLastRenewed > captchaValidUntil) 
				{
					await Navigation.PopModalAsync();
				}else
				{
					GetCaptcha(account);
				}
			};			
		}

		async void GetCaptcha(AccountModel account)
		{
			var s = new LacunaExpress.Data.Server();
			var json = Captcha.Fetch(account.SessionID);
			var response = await s.GetHttpResultAsync(account.Server, Captcha.url, json);
			if (response.result != null)
			{
				guid = response.result.guid;
				captchaImage.Source = response.result.url;
			}
		}
	}	
}
