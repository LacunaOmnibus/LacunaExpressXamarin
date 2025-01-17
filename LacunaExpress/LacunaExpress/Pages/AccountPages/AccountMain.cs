﻿using LacunaExpress.AccountManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

using LacunaExpress.Styles;

namespace LacunaExpress.Pages.AccountPages
{
	public class AccountMain : ContentPage
	{
		AccountCollection accounts = new AccountCollection();
		AccountModel selectedAccount = new AccountModel();

		AccountManagement.AccountManager accountManager = new AccountManagement.AccountManager();
		Button addAccount = new Button
		{
			Text = "Add Account",
			Style = (Style)Styles.Styles.StyleDictionary[Styles.Styles.StyleName.RegularButton.ToString()]
		};
		Button modifyAccount = new Button
		{
			Text = "Modify Account",
			Style = (Style)Styles.Styles.StyleDictionary[Styles.Styles.StyleName.RegularButton.ToString()]
		};	
		Button deletAccount = new Button
		{
			Text = "Delete Account",
			Style = (Style)Styles.Styles.StyleDictionary[Styles.Styles.StyleName.RegularButton.ToString()]
				
		};
		Picker accountPicker = new Picker
			{
				Title = "Select Account",
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
		public AccountMain()
		{
			var mainLayout = new StackLayout
			{
				BackgroundColor = Color.FromRgb (0, 0, 128),

				Padding = new Thickness(6, 6, 6, 6),
				Children = {
					accountPicker,
					addAccount,
					deletAccount
				}
			};

			Content = mainLayout;
			if (Device.OS == TargetPlatform.iOS)
			{
				mainLayout.Padding = new Thickness (0, 20, 0, 0);
			}

			deletAccount.Clicked += (sender, e) =>
			{
				accountManager.ClearAccount();
			};
			accountPicker.SelectedIndexChanged += async (sender, e) =>
			{
				selectedAccount = accounts.AccountList.Where(x => x.DisplayName.Equals(accountPicker.Items[accountPicker.SelectedIndex])).First();
				var oldAccount = selectedAccount;
				selectedAccount.ActiveAccount = true;
				accountManager.ModifyAccountAsync(selectedAccount, oldAccount);
			};
			this.Appearing += (sender, e)  =>
			{
				LoadAccountsIntoPicker();
			};
			

			addAccount.Clicked += async (sender, e) =>
			{
				await Navigation.PushModalAsync(new LacunaExpress.Pages.AccountPages.Login());
			};
		}

		async void LoadAccountsIntoPicker()
		{
			AccountManagement.AccountManager accountManger = new AccountManagement.AccountManager();
			accounts = await accountManager.LoadAccountsAsync();
			selectedAccount = await accountManager.GetActiveAccountAsync();
			if (accounts != null)
			{
				foreach (var a in accounts.AccountList)
					accountPicker.Items.Add(a.DisplayName);
			}
			else
			{
				await Navigation.PushModalAsync(new LacunaExpress.Pages.AccountPages.Login());
			}
		}
		
	}
}
