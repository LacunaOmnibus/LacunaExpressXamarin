﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;
using LacunaExpanseAPIWrapper;
using LacunaExpress.Data;
using LacunaExpanseAPIWrapper.ResponseModels;

using LacunaExpress.Styles;

namespace LacunaExpress.Pages.Mail
{
	public class ReadMessage : ContentPage
	{
		Label Date    = new Label { TextColor = Color.White, BackgroundColor = Color.FromRgb (0, 0, 128) };
		Label From    = new Label { TextColor = Color.White, BackgroundColor = Color.FromRgb (0, 0, 128) };
		Label To      = new Label { TextColor = Color.White, BackgroundColor = Color.FromRgb (0, 0, 128) };
		Label Subject = new Label { TextColor = Color.White, BackgroundColor = Color.FromRgb (0, 0, 128) };
		Label Message = new Label { TextColor = Color.White, BackgroundColor = Color.FromRgb (0, 0, 128) };

		Button Reply = new Button 
		{ 
			Text = "Reply",
			Style = (Style)Styles.Styles.StyleDictionary[Styles.Styles.StyleName.RegularButton.ToString()]
		};
		Button Archive = new Button
		{
			Text = "Archive",
			Style = (Style)Styles.Styles.StyleDictionary[Styles.Styles.StyleName.RegularButton.ToString()]
		};
		Button Forward = new Button
		{
			Text = "Forward",
			Style = (Style)Styles.Styles.StyleDictionary[Styles.Styles.StyleName.RegularButton.ToString()]
		};
		Button Delete = new Button
		{
			Text = "Delete",
			Style = (Style)Styles.Styles.StyleDictionary[Styles.Styles.StyleName.RegularButton.ToString()]
		};
		
		public ReadMessage(string sessionID, string server, Messages message)
		{
			Date.Text = message.date;
			From.Text = message.from;
			To.Text = message.to;
			Subject.Text = message.subject;
			Message.Text = message.body;
			Message.BackgroundColor = Color.FromRgb (0, 0, 128);
			var mainLayout = new StackLayout
			{
				Children = {
					Date, From, To, Subject,
					new ScrollView{
						Content = Message,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                    },
					new StackLayout{
						
						Orientation = StackOrientation.Horizontal,
						Children = {Reply, Archive, Forward, Delete},
                        VerticalOptions = LayoutOptions.End, 
						BackgroundColor = Color.FromRgb (0, 0, 128),
                    }
				}
			};

			Content = mainLayout;
			if (Device.OS == TargetPlatform.iOS)
			{
				mainLayout.Padding = new Thickness (0, 20, 0, 0);
			}

			Reply.Clicked += async (sender, e) =>{
				//await Navigation.PopAsync();
				await Navigation.PushAsync(new ComposeReply(sessionID, server, message, "Re: "));
			};
			Archive.Clicked += async (sender, e) => 
			{
				var json = Inbox.ArchiveMessages(1, sessionID, message.id);
				var s = new LacunaExpress.Data.Server();
				var response = await s.GetHttpResultAsync(server, Inbox.url, json);
				await Navigation.PopAsync();
			};
			Forward.Clicked += async (sender, e) => 
			{
				//await Navigation.PopAsync();
				await Navigation.PushAsync(new ComposeReply(sessionID, server, message, "Fw: "));
			};
			Delete.Clicked += async (sender, e) => {
				var json = Inbox.TrashMessages(1, sessionID, message.id);
				var s = new LacunaExpress.Data.Server();
				var response = await s.GetHttpResultAsync(server, Inbox.url, json);
				await Navigation.PopAsync();
			};
		}
	}
}
