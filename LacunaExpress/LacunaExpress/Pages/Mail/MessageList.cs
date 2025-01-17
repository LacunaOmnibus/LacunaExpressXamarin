﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using System.Collections.ObjectModel;

using Xamarin.Forms;
using LacunaExpanseAPIWrapper;

using LacunaExpress.AccountManagement;
using LacunaExpress.Data;
using LacunaExpress.Pages.AccountPages;
using System.Threading.Tasks;

using LacunaExpress.Styles;

namespace LacunaExpress.Pages.Mail
{

	public class MessageList : ContentPage
	{
		AccountModel account = null;
		ObservableCollection<MessageData> messageList = new ObservableCollection<MessageData>();
		string sessionID = "";
		List<string> messageCategoryList = new List<string> 
		{ 
			"All", "Correspondence", "Archive", "Trashed", "Sent",
			"Tutorial", "Medal", "Intelligence", "Alert", "Attack", "Colonization", "Complaint", "Excavator", "Mission", 
			"Parliament", "Probe", "Spies", "Trade", "Fissure"
		};

		Picker messageCategories;
		ListView messages = new ListView { HasUnevenRows = true };
		Button compose = new Button
		{
			Text = "Compose",
			Style = (Style)Styles.Styles.StyleDictionary[Styles.Styles.StyleName.RegularButton.ToString()]
		};

		async Task<bool> LoadMessagesAsync(string category)
		{
			var acntMgr = new AccountManagement.AccountManager();
			account = await acntMgr.GetActiveAccountAsync();
			if (account == null)
			{
				await Navigation.PushAsync(new Login());
				return false;
			}
			else
			{
				messageList.Clear();
                string json;
				switch (category)
				{
					case "All":
						json = Inbox.ViewInbox(account.SessionID);
						break;
					case "Sent":
						json = Inbox.ViewSent(account.SessionID);
						break;
					case "Trashed":
						json = Inbox.ViewTrash(account.SessionID);
						break;
					case "Archived":
						json = Inbox.ViewArchive(account.SessionID);
						break;
					default:
						json = Inbox.ViewInbox(account.SessionID, category);
						break;
				}
				
								
				try
				{
					var s = new Server();
					var response = await s.GetHttpResultAsync(account.Server, Inbox.url, json);
					foreach (var m in response.result.messages)
						messageList.Add(new MessageData(m.from, m.subject, m.body_preview, m.id.ToString()));
				}
				catch (Exception e) { var f = e.Message; }
			}
			return true;
		}

		public MessageList()
		{
			messageCategories = new Picker
			{
				Title = "Message Categories",
				VerticalOptions = LayoutOptions.CenterAndExpand
			};


			foreach (var s in messageCategoryList)
			{
				messageCategories.Items.Add(s);
			}

			messageList.Add(new MessageData("Messages Loading", "Messages Loading", "Messages Loading", "Messages Loading"));
			

			messages.HasUnevenRows = true;
			messages.IsVisible = true;
			messages.SeparatorColor = Color.Red;
			messages.BackgroundColor = Color.FromRgb (0, 0, 128);

			var mainLayout = new StackLayout
			{
				BackgroundColor = Color.FromRgb (0, 0, 128),
				Children = {
					messages,
					
					messageCategories,
					compose
					//new Label { Text = "" }
				}
			};

			Content = mainLayout;
			if (Device.OS == TargetPlatform.iOS)
			{
				mainLayout.Padding = new Thickness (0, 20, 0, 0);
			}

			this.Appearing += async (sender, e) =>{
				await LoadMessagesAsync("All");
			};


			messages.ItemTemplate = new DataTemplate(typeof(MenuItem));
			if (messageList.Count > 0)
			{
				messages.ItemsSource = messageList;
			}
			messages.ItemSelected += async (sender, e) =>
			{
				var json = Inbox.ReadMessage(account.SessionID, (e.SelectedItem as MessageData).MessageID);
				var s = new Server();
				var response = await s.GetHttpResultAsync(account.Server, Inbox.url, json);
				if(response.result != null)
					await Navigation.PushAsync(new ReadMessage(account.SessionID, account.Server, response.result.message));
			};


			messageCategories.SelectedIndexChanged += async (sender, e) =>
			{
				await LoadMessagesAsync(messageCategories.Items[messageCategories.SelectedIndex]);
			};

			compose.Clicked += async (sender, e) =>
			{
				await Navigation.PushAsync(new ComposeReply(account.SessionID, account.Server));
			};

		}



		async Task<AccountModel> GetActiveAccountAsync()
		{
			var accm = new AccountManagement.AccountManager();
			var account = await accm.GetActiveAccountAsync();
			if (account == null)
			{
				await Navigation.PushModalAsync(new Login());
				account = await GetActiveAccountAsync();
			}
			return account;
		}
	}

	class MenuItem : ViewCell
	{
		//StackLayout OuterHorizontal = new StackLayout { Orientation = StackOrientation.Horizontal };
		StackLayout VerticalInner = new StackLayout { Orientation = StackOrientation.Vertical };

		Label GoogleLetter = new Label { TextColor = Color.White };
		Label From = new Label { TextColor = Color.White };
		Label MessageHeader = new Label { TextColor = Color.White };
		Label BodySummary = new Label { TextColor = Color.White };

		public MenuItem()
		{
			From.SetBinding(Label.TextProperty, "From");
			MessageHeader.SetBinding(Label.TextProperty, "Subject");
			VerticalInner.Children.Add(From);
			VerticalInner.Children.Add(MessageHeader);
			View = VerticalInner;
		}
	}

	class MessageData
	{
		public MessageData(string from, string subject, string bodyPreview, string messageID)
		{
			From = from;
			Subject = subject;
			BodyPreview = bodyPreview;
			MessageID = messageID;
		}
		public string From { get; set; }
		public string Subject { get; set; }
		public string BodyPreview { get; set; }
		public string MessageID { get; set; }
	}
}
