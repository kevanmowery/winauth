﻿/*
 * Copyright (C) 2013 Colin Mackie.
 * This software is distributed under the terms of the GNU General Public License.
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WinAuth
{
	public class AuthenticatorListitem
	{
		public AuthenticatorListitem(WinAuthAuthenticator auth, int index)
		{
			Authenticator = auth;
			LastUpdate = DateTime.MinValue;
			Index = index;
			DisplayUntil = DateTime.MinValue;
		}

		public int Index { get; set; }
		public WinAuthAuthenticator Authenticator { get; set; }
		public DateTime LastUpdate { get; set;}
		public DateTime DisplayUntil { get; set; }
		public string LastCode { get; set; }
	}

	public delegate void AuthenticatorListItemRemovedHandler(object source, AuthenticatorListItemRemovedEventArgs args);

	public class AuthenticatorListItemRemovedEventArgs : EventArgs
	{
		public AuthenticatorListitem Item { get; private set; }

		/// <summary>
		/// Default constructor
		/// </summary>
		public AuthenticatorListItemRemovedEventArgs(AuthenticatorListitem item)
			: base()
		{
			Item = item;
		}
	}

  public class AuthenticatorListBox : ListBox
  {
		private const int WM_HSCROLL = 0x114;
		private const int WM_VSCROLL = 0x115;

		private const int SB_LINELEFT = 0;
		private const int SB_LINERIGHT = 1;
		private const int SB_PAGELEFT = 2;
		private const int SB_PAGERIGHT = 3;
		private const int SB_THUMBPOSITION = 4;
		private const int SB_THUMBTRACK = 5;
		private const int SB_LEFT = 6;
		private const int SB_RIGHT = 7;
		private const int SB_ENDSCROLL = 8;

		private const int SIF_TRACKPOS = 0x10;
		private const int SIF_RANGE = 0x1;
		private const int SIF_POS = 0x4;
		private const int SIF_PAGE = 0x2;
		private const int SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS;

		[DllImport("user32.dll", SetLastError = true)]
		private static extern int GetScrollInfo( IntPtr hWnd, int n, ref ScrollInfoStruct lpScrollInfo);

		private struct ScrollInfoStruct
		{
			public int cbSize;
			public int fMask;
			public int nMin;
			public int nMax;
			public int nPage;
			public int nPos;
			public int nTrackPos;
		}

		public AuthenticatorListBox()
    {
      this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
			this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.ReadOnly = true;

			this.Scrolled += AuthenticatorListBox_Scrolled;

			this.ContextMenuStrip = new ContextMenuStrip();
			this.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

			loadContextMenuStrip();
    }

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			setRenameTextboxLocation();
		}

		void AuthenticatorListBox_Scrolled(object sender, ScrollEventArgs e)
		{
			if (e.Type == ScrollEventType.EndScroll || e.Type == ScrollEventType.ThumbPosition)
			{
				setRenameTextboxLocation();
			}
		}

		private void setRenameTextboxLocation()
		{
			if (_renameTextbox != null && _renameTextbox.Visible == true)
			{
				AuthenticatorListitem item = _renameTextbox.Tag as AuthenticatorListitem;
				if (item != null)
				{
					int y = (this.ItemHeight * item.Index) - (this.TopIndex * this.ItemHeight) + 8;
					if (RenameTextbox.Location.Y != y)
					{
						RenameTextbox.Location = new Point(RenameTextbox.Location.X, y);
					}
					Refresh();
				}
			}
		}

		public event AuthenticatorListItemRemovedHandler ItemRemoved;

		[Category("Action")]
		public event ScrollEventHandler Scrolled = null;

		private TextBox _renameTextbox;

		public TextBox RenameTextbox
		{
			get
			{
				if (_renameTextbox == null)
				{
					_renameTextbox = new TextBox();
					_renameTextbox.Name = "renameTextBox";
					_renameTextbox.AllowDrop = true;
					_renameTextbox.CausesValidation = false;
					_renameTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
					_renameTextbox.Location = new System.Drawing.Point(0, 0);
					_renameTextbox.Multiline = false;
					_renameTextbox.Name = "secretCodeField";
					_renameTextbox.Size = new System.Drawing.Size(250, 22);
					_renameTextbox.TabIndex = 0;
					_renameTextbox.Visible = false;
					_renameTextbox.Leave += RenameTextbox_Leave;
					_renameTextbox.KeyPress += _renameTextbox_KeyPress;

					this.Controls.Add(_renameTextbox);
				}

				return _renameTextbox;
			}
		}

		void _renameTextbox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 27)
			{
				RenameTextbox.Tag = null;
				RenameTextbox.Visible = false;
				e.Handled = true;
			}
			else if (e.KeyChar == 13 || e.KeyChar == 9)
			{
				RenameTextbox.Visible = false;
				e.Handled = true;
			}
		}

		void RenameTextbox_Leave(object sender, EventArgs e)
		{
			RenameTextbox.Visible = false;
			AuthenticatorListitem item = RenameTextbox.Tag as AuthenticatorListitem;
			if (item != null)
			{
				string newname = RenameTextbox.Text.Trim();
				if (newname.Length != 0)
				{
					item.Authenticator.Name = newname;
					RefreshItem(item.Index);
				}
			}
		}

		protected override void WndProc(ref System.Windows.Forms.Message msg)
		{
			if (msg.Msg == WM_VSCROLL)
			{
				if (Scrolled != null)
				{
					ScrollInfoStruct si = new ScrollInfoStruct();
					si.fMask = SIF_ALL;
					si.cbSize = Marshal.SizeOf(si);
					GetScrollInfo(msg.HWnd, 0, ref si);

					if (msg.WParam.ToInt32() == SB_ENDSCROLL)
					{
						ScrollEventArgs sargs = new ScrollEventArgs(ScrollEventType.EndScroll, si.nPos);
						Scrolled(this, sargs);
					}
					else if (msg.WParam.ToInt32() == SB_THUMBTRACK)
					{
						ScrollEventArgs sargs = new ScrollEventArgs(ScrollEventType.ThumbTrack, si.nPos);
						Scrolled(this, sargs);
					}
				}
			}

			base.WndProc(ref msg);
		}

		private AuthenticatorListitem _currentItem;

		public AuthenticatorListitem CurrentItem
		{
			get
			{
				return _currentItem;
			}
			set
			{
				_currentItem = value;
			}
		}

		private void SetCurrentItem(Point mouseLocation)
		{
			int index = this.IndexFromPoint(mouseLocation);
			if (index < 0)
			{
				index = 0;
			}
			else if (index >= this.Items.Count)
			{
				index = this.Items.Count - 1;
			}

			if (index >= this.Items.Count)
			{
				CurrentItem = null;
			}
			else
			{
				CurrentItem = this.Items[index] as AuthenticatorListitem;
			}
		}

		private void SetCursor(Point mouseLocation)
		{
			// set cursor if we are over a refresh icon
			var cursor = Cursor.Current;
			int index = this.IndexFromPoint(mouseLocation);
			if (index >= 0 && index < this.Items.Count)
			{
				var item = this.Items[index] as AuthenticatorListitem;
				int x = 0;
				int y = this.ItemHeight * index;
				if (item.Authenticator.AutoRefresh == false && item.DisplayUntil < DateTime.Now
					&& new Rectangle(x + this.Width - 56, y + 8, 48, 48).Contains(mouseLocation))
				{
					cursor = Cursors.Hand;
				}
			}
			if (Cursor.Current != cursor)
			{
				Cursor.Current = cursor;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			//if (this.ClientRectangle.Contains(this.PointToClient(Control.MousePosition)))
			//{
			//	return;
			//}

			SetCursor(e.Location);

			base.OnMouseMove(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			SetCurrentItem(e.Location);
			SetCursor(e.Location);

			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if ((e.Button & System.Windows.Forms.MouseButtons.Left) != 0)
			{
				// if this was in a refresh icon, we do a refresh
				int index = this.IndexFromPoint(e.Location);
				if (index >= 0 && index < this.Items.Count)
				{
					var item = this.Items[index] as AuthenticatorListitem;
					int x = 0;
					int y = this.ItemHeight * index;
					if (item.Authenticator.AutoRefresh == false && item.DisplayUntil < DateTime.Now
						&& new Rectangle(x + this.Width - 56, y + 8, 48, 48).Contains(e.Location))
					{
						item.LastUpdate = DateTime.Now;
						item.DisplayUntil = DateTime.Now.AddSeconds(10);

						if (item.Authenticator.CopyOnCode == true)
						{
							// copy to clipboard
							item.Authenticator.CopyCodeToClipboard(this.Parent as Form);
						}

						RefreshCurrentItem();
					}
				}
			}
		}

		private void loadContextMenuStrip()
		{
			this.ContextMenuStrip.Items.Clear();

			ToolStripLabel label = new ToolStripLabel();
			label.Name = "contextMenuItemName";
			label.ForeColor = SystemColors.HotTrack;
			this.ContextMenuStrip.Items.Add(label);
			this.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			//
			EventHandler onclick = new EventHandler(ContextMenu_Click);
			//
			ToolStripMenuItem menuitem;
			ToolStripMenuItem subitem;
			//
			menuitem = new ToolStripMenuItem("Show Code");
			menuitem.Name = "showCodeMenuItem";
			menuitem.Click += ContextMenu_Click;
			this.ContextMenuStrip.Items.Add(menuitem);
			//
			menuitem = new ToolStripMenuItem("Copy Code");
			menuitem.Name = "copyCodeMenuItem";
			menuitem.Click += ContextMenu_Click;
			this.ContextMenuStrip.Items.Add(menuitem);
			//
			this.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			//
			menuitem = new ToolStripMenuItem("Show Serial && Restore Code...");
			menuitem.Name = "showRestoreCodeMenuItem";
			menuitem.Click += ContextMenu_Click;
			this.ContextMenuStrip.Items.Add(menuitem);
			//
			menuitem = new ToolStripMenuItem("Show Secret Key...");
			menuitem.Name = "showGoogleSecretMenuItem";
			menuitem.Click += ContextMenu_Click;
			this.ContextMenuStrip.Items.Add(menuitem);
			//
			menuitem = new ToolStripMenuItem("Show Serial Key and Device ID...");
			menuitem.Name = "showTrionSecretMenuItem";
			menuitem.Click += ContextMenu_Click;
			this.ContextMenuStrip.Items.Add(menuitem);
			//
			this.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			//
			menuitem = new ToolStripMenuItem("Auto Refresh");
			menuitem.Name = "autoRefreshMenuItem";
			menuitem.Click += ContextMenu_Click;
			this.ContextMenuStrip.Items.Add(menuitem);
			//
			this.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			//
			menuitem = new ToolStripMenuItem("Delete");
			menuitem.Name = "deleteMenuItem";
			menuitem.Click += ContextMenu_Click;
			this.ContextMenuStrip.Items.Add(menuitem);
			//
			menuitem = new ToolStripMenuItem("Rename");
			menuitem.Name = "renameMenuItem";
			menuitem.Click += ContextMenu_Click;
			this.ContextMenuStrip.Items.Add(menuitem);
			//
			this.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			//
			menuitem = new ToolStripMenuItem("Copy On New Code");
			menuitem.Name = "copyOnCodeMenuItem";
			menuitem.Click += ContextMenu_Click;
			this.ContextMenuStrip.Items.Add(menuitem);
			//
			menuitem = new ToolStripMenuItem("Icon");
			menuitem.Name = "iconMenuItem";
			subitem = new ToolStripMenuItem();
			subitem.Text = "Auto";
			subitem.Name = "iconMenuItem_default";
			subitem.Tag = string.Empty;
			subitem.Click += ContextMenu_Click;
			menuitem.DropDownItems.Add(subitem);
			menuitem.DropDownItems.Add("-");
			this.ContextMenuStrip.Items.Add(menuitem);
			int iconindex = 1;
			foreach (string icon in WinAuthMain.AUTHENTICATOR_ICONS.Keys)
			{
				string iconfile = WinAuthMain.AUTHENTICATOR_ICONS[icon];
				if (iconfile.Length == 0)
				{
					menuitem.DropDownItems.Add(new ToolStripSeparator());
				}
				else
				{
					subitem = new ToolStripMenuItem();
					subitem.Text = icon;
					subitem.Name = "iconMenuItem_" + iconindex++;
					subitem.Tag = iconfile;
					subitem.Image = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("WinAuth.Resources." + iconfile));
					subitem.ImageAlign = ContentAlignment.MiddleLeft;
					subitem.ImageScaling = ToolStripItemImageScaling.SizeToFit;
					subitem.Click += ContextMenu_Click;
					menuitem.DropDownItems.Add(subitem);
				}
			}
			menuitem.DropDownItems.Add("-");
			subitem = new ToolStripMenuItem();
			subitem.Text = "Other...";
			subitem.Name = "iconMenuItem_0";
			subitem.Tag = "OTHER";
			subitem.Click += ContextMenu_Click;
			menuitem.DropDownItems.Add(subitem);
			this.ContextMenuStrip.Items.Add(menuitem);
			//
			this.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			//
			menuitem = new ToolStripMenuItem("Sync Time");
			menuitem.Name = "syncMenuItem";
			menuitem.Click += ContextMenu_Click;
			this.ContextMenuStrip.Items.Add(menuitem);
		}

		void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			var menu = this.ContextMenuStrip;
			var item = this.CurrentItem;
			WinAuthAuthenticator auth = null;
			if (item == null || (auth = item.Authenticator) == null)
			{
				return;
			}

			ToolStripLabel labelitem = menu.Items.Cast<ToolStripItem>().Where(i => i.Name == "contextMenuItemName").FirstOrDefault() as ToolStripLabel;
			labelitem.Text = item.Authenticator.Name;

			ToolStripMenuItem menuitem = menu.Items.Cast<ToolStripItem>().Where(i => i.Name == "showCodeMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.Visible = !auth.AutoRefresh;
			//
			menuitem = menu.Items.Cast<ToolStripItem>().Where(i => i.Name == "copyCodeMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.Enabled = !(auth.AutoRefresh == false && item.DisplayUntil < DateTime.Now);
			//
			menuitem = menu.Items.Cast<ToolStripItem>().Where(i => i.Name == "showRestoreCodeMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.Visible = (auth.AuthenticatorData is BattleNetAuthenticator);
			//
			menuitem = menu.Items.Cast<ToolStripItem>().Where(i => i.Name == "showGoogleSecretMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.Visible = (auth.AuthenticatorData is GoogleAuthenticator);
			//
			menuitem = menu.Items.Cast<ToolStripItem>().Where(i => i.Name == "showTrionSecretMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.Visible = (auth.AuthenticatorData is TrionAuthenticator);
			//
			menuitem = menu.Items.Cast<ToolStripItem>().Where(i => i.Name == "autoRefreshMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.CheckState = (auth.AutoRefresh == true ? CheckState.Checked : CheckState.Unchecked);
			//
			menuitem = menu.Items.Cast<ToolStripItem>().Where(i => i.Name == "copyOnCodeMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.CheckState = (auth.CopyOnCode == true ? CheckState.Checked : CheckState.Unchecked);
			//
			menuitem = menu.Items.Cast<ToolStripItem>().Where(i => i.Name == "iconMenuItem").FirstOrDefault() as ToolStripMenuItem;
			ToolStripMenuItem subitem = menuitem.DropDownItems.Cast<ToolStripItem>().Where(i => i.Name == "iconMenuItem_default").FirstOrDefault() as ToolStripMenuItem;
			subitem.CheckState = CheckState.Checked;
			foreach (ToolStripItem iconitem in menuitem.DropDownItems)
			{
				if (iconitem is ToolStripMenuItem)
				{
					ToolStripMenuItem iconmenuitem = (ToolStripMenuItem)iconitem;
					if (string.IsNullOrEmpty((string)iconmenuitem.Tag) && string.IsNullOrEmpty(auth.Skin) == true)
					{
						iconmenuitem.CheckState = CheckState.Checked;
					}
					else if (string.Compare((string)iconmenuitem.Tag, auth.Skin) == 0)
					{
						iconmenuitem.CheckState = CheckState.Checked;
					}
					else
					{
						iconmenuitem.CheckState = CheckState.Unchecked;
					}
				}
			}
		}

		//void ContextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		//{
		//	// display any resources
		//	var menuitem = this.ContextMenuStrip.Items.Cast<ToolStripItem>().Where(i => i is ToolStripMenuItem && ((ToolStripMenuItem)i).Name == "iconMenuItem").FirstOrDefault() as ToolStripMenuItem;
		//	if (menuitem != null)
		//	{
		//		foreach (ToolStripItem subitem in menuitem.DropDownItems)
		//		{
		//			if (subitem.Image != null)
		//			{
		//				subitem.Image.Dispose();
		//				subitem.Image = null;
		//			}
		//		}
		//	}
		//}

		void ContextMenu_Click(object sender, EventArgs e)
		{
			ToolStripItem menuitem = (ToolStripItem)sender;
			var item = this.CurrentItem;
			var auth = item.Authenticator;

			if (menuitem.Name == "showCodeMenuItem")
			{
				item.LastUpdate = DateTime.Now;
				item.DisplayUntil = DateTime.Now.AddSeconds(10);
				RefreshCurrentItem();
			}
			else if (menuitem.Name == "syncMenuItem")
			{
				item.Authenticator.AuthenticatorData.Sync();
				item.LastUpdate = DateTime.MinValue;
				RefreshCurrentItem();
			}
			else if (menuitem.Name == "copyCodeMenuItem")
			{
				auth.CopyCodeToClipboard(this.Parent as Form, item.LastCode);
			}
			else if (menuitem.Name == "autoRefreshMenuItem")
			{
				auth.AutoRefresh = !auth.AutoRefresh;
				item.LastUpdate = DateTime.Now;
				item.DisplayUntil = DateTime.MinValue;
				RefreshCurrentItem();
			}
			else if (menuitem.Name == "copyOnCodeMenuItem")
			{
				auth.CopyOnCode = !auth.CopyOnCode;
			}
			else if (menuitem.Name == "showRestoreCodeMenuItem")
			{
				// show the serial and restore code for Battle.net authenticator				
				ShowRestoreCodeForm form = new ShowRestoreCodeForm();
				form.CurrentAuthenticator = auth;
				form.ShowDialog(this.Parent as Form);
			}
			else if (menuitem.Name == "showGoogleSecretMenuItem")
			{
				// show the secret key for Google authenticator				
				ShowSecretKeyForm form = new ShowSecretKeyForm();
				form.CurrentAuthenticator = auth;
				form.ShowDialog(this.Parent as Form);
			}
			else if (menuitem.Name == "showTrionSecretMenuItem")
			{
				// show the secret key for Trion authenticator				
				ShowTrionSecretForm form = new ShowTrionSecretForm();
				form.CurrentAuthenticator = auth;
				form.ShowDialog(this.Parent as Form);
			}
			else if (menuitem.Name == "deleteMenuItem")
			{
				if (WinAuthForm.ConfirmDialog(this.Parent as Form,
					"Are you sure you want to delete this authenticator?" + Environment.NewLine + Environment.NewLine
					+ "This will permanently remove it and you may no longer be able to access you account.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
				{
					int index = item.Index;
					this.Items.Remove(item);
					ItemRemoved(this, new AuthenticatorListItemRemovedEventArgs(item));
					if (index >= this.Items.Count)
					{
						index = this.Items.Count - 1;
					}
					this.CurrentItem = (this.Items.Count != 0 ? this.Items[index] as AuthenticatorListitem : null);
				}
			}
			else if (menuitem.Name == "renameMenuItem")
			{
				int y = (this.ItemHeight * item.Index) - (this.TopIndex * this.ItemHeight) + 8;
				RenameTextbox.Location = new Point(64, y);
				RenameTextbox.Text = auth.Name;
				RenameTextbox.Tag = item;
				RenameTextbox.Visible = true;
				RenameTextbox.Focus();
			}
			else if (menuitem.Name.StartsWith("iconMenuItem_") == true)
			{
				if (menuitem.Tag is string && string.Compare((string)menuitem.Tag, "OTHER") == 0)
				{
					do
					{
						// other..choose an image file
						OpenFileDialog ofd = new OpenFileDialog();
						ofd.AddExtension = true;
						ofd.CheckFileExists = true;
						ofd.DefaultExt = "png";
						ofd.InitialDirectory = Directory.GetCurrentDirectory();
						ofd.FileName = string.Empty;
						ofd.Filter = "PNG Image Files (*.png)|*.png|GIF Image Files (*.gif)|*.gif|All Files (*.*)|*.*";
						ofd.RestoreDirectory = true;
						ofd.ShowReadOnly = false;
						ofd.Title = "Load Icon Image (png or gif @ 48x48)";
						DialogResult result = ofd.ShowDialog(this);
						if (result != System.Windows.Forms.DialogResult.OK)
						{
							return;
						}
						try
						{
							using (Bitmap iconimage = (Bitmap)Image.FromFile(ofd.FileName))
							{
								if (iconimage.Width != 48 || iconimage.Height != 48)
								{
									Image.GetThumbnailImageAbort thumbNailCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
									using (Bitmap scaled = iconimage.GetThumbnailImage(48, 48, thumbNailCallback, System.IntPtr.Zero) as Bitmap)
									{
										auth.Icon = scaled;
									}
								}
								else
								{
									auth.Icon = iconimage;
								}
								RefreshCurrentItem();
							}
						}
						catch (Exception ex)
						{
							if (MessageBox.Show(this.Parent as Form,
								"Error loading image file: " + ex.Message + Environment.NewLine + Environment.NewLine + "Do you want to try again?",
								WinAuthMain.APPLICATION_NAME,
								MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
							{
								continue;
							}
						}
						break;
					} while (true);
				}
				else 
				{
					auth.Skin = (((string)menuitem.Tag).Length != 0 ? (string)menuitem.Tag : null);
					RefreshCurrentItem();
				}
			}
		}

		public bool ThumbnailCallback()
		{
			return false;
		}

		private Bitmap ScaleBitmap(Bitmap bitmap, int width, int height)
		{
			Bitmap scaled = new Bitmap(width, height, PixelFormat.Format24bppRgb);
			float scalex = (float)width / (float)bitmap.Width;
			float scaley = (float)Height / (float)bitmap.Height;

			using (Graphics g = Graphics.FromImage((Image)scaled))
			{
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
				g.ScaleTransform(scalex, scaley);

				Rectangle destrectangle = new Rectangle(0, 0, scaled.Size.Width, scaled.Size.Height);
				Rectangle srcrectangle = new Rectangle(0, 0, bitmap.Size.Width, bitmap.Size.Height);
				g.DrawImage(bitmap, destrectangle, srcrectangle, GraphicsUnit.Pixel);
			}

			return scaled;
		}

		private void RefreshCurrentItem()
		{
			var item = this.CurrentItem;
			int y = this.ItemHeight * item.Index;
			Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
			// Rectangle rect = new Rectangle(this.Width - 56, y + 8, 48, 48);
			this.Invalidate(rect, false);
		}

		public void Tick(object sender, EventArgs e)
		{
			for (int index = 0; index < this.Items.Count; index++)
			{
				AuthenticatorListitem item = this.Items[index] as AuthenticatorListitem;
				WinAuthAuthenticator auth = item.Authenticator;

				int y = this.ItemHeight * index;
				if (auth.AutoRefresh == true)
				{
					int tillUpdate = (int)((auth.AuthenticatorData.ServerTime % 30000) / 1000L);
					if (item.LastUpdate == DateTime.MinValue || tillUpdate == 0)
					{
						this.Invalidate(new Rectangle(0, y, this.Width, this.ItemHeight), false);
						item.LastUpdate = DateTime.Now;
					}
					else
					{
						Rectangle rect = new Rectangle(this.Width - 56, y + 8, 48, 48);
						this.Invalidate(rect, false);
						item.LastUpdate = DateTime.Now;
					}
				}
				else
				{
					if (item.DisplayUntil != DateTime.MinValue)
					{
						if (item.DisplayUntil <= DateTime.Now)
						{
							item.DisplayUntil = DateTime.MinValue;
							item.LastUpdate = DateTime.MinValue;
							item.LastCode = null;

							SetCursor(this.PointToClient(Control.MousePosition));
						}
						this.Invalidate(new Rectangle(0, y, this.Width, this.ItemHeight), false);
					}
				}
			}
		}

		public bool ReadOnly { get; set; }

		protected override void DefWndProc(ref Message m)
		{
			if (ReadOnly == false || ((m.Msg <= 0x0200 || m.Msg >= 0x020E)
				&& (m.Msg <= 0x0100 || m.Msg >= 0x0109)
				&& m.Msg != 0x2111
				&& m.Msg != 0x87))
			{
				base.DefWndProc(ref m);
			}
		}

		protected void OnDrawItem(DrawItemEventArgs e, Rectangle cliprect)
		{
			if (this.Items.Count > 0)
			{
				using (var brush = new SolidBrush(e.ForeColor))
				{
					AuthenticatorListitem item = this.Items[e.Index] as AuthenticatorListitem;
					WinAuthAuthenticator auth = item.Authenticator;

					bool showCode = (auth.AutoRefresh == true || item.DisplayUntil > DateTime.Now);

					e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

					Rectangle rect = new Rectangle(e.Bounds.X + 4, e.Bounds.Y + 8, 48, 48);
					if (cliprect.IntersectsWith(rect) == true)
					{
						using (var icon = auth.Icon)
						{
							e.Graphics.DrawImage(icon, new PointF(e.Bounds.X + 4, e.Bounds.Y + 8));
						}
					}

					using (var font = new Font(e.Font.FontFamily, 12, FontStyle.Regular))
					{
						string label = (e.Index + 1) + ". " + auth.Name;
						SizeF labelsize = e.Graphics.MeasureString(label.ToString(), font);
						// if too big, adjust
						if (labelsize.Width > 255)
						{
							StringBuilder newlabel = new StringBuilder(label + "...");
							while ((labelsize = e.Graphics.MeasureString(newlabel.ToString(), font)).Width > 255)
							{
								newlabel.Remove(newlabel.Length - 4, 1);
							}
							label = newlabel.ToString();
						}
						rect = new Rectangle(e.Bounds.X + 64, e.Bounds.Y + 8, (int)labelsize.Height, (int)labelsize.Width);
						if (cliprect.IntersectsWith(rect) == true)
						{
							//e.Graphics.DrawString(label, font, brush, new PointF(e.Bounds.X + 64, e.Bounds.Y + 8));
							e.Graphics.DrawString(label, font, brush, new RectangleF(e.Bounds.X + 64, e.Bounds.Y + 8, 250, labelsize.Height));
						}

						string code;
						if (showCode == true)
						{
							// we we aren't autorefresh we just keep the same code up for the 10 seconds so it doesn't change even crossing the 30s boundary
							if (auth.AutoRefresh == false)
							{
								if (item.LastCode == null)
								{
									code = auth.AuthenticatorData.CurrentCode;
								}
								else
								{
									code = item.LastCode;
								}
							}
							else
							{
								code = auth.AuthenticatorData.CurrentCode;
								if (code != item.LastCode && auth.CopyOnCode == true)
								{
									// code has changed - copy to clipboard
									auth.CopyCodeToClipboard(this.Parent as Form);
								}
							}
							item.LastCode = code;
							code = code.Insert(code.Length / 2, " ");
						}
						else
						{
							code = "- - - - - -";
						}
						SizeF codesize = e.Graphics.MeasureString(code, e.Font);
						rect = new Rectangle(e.Bounds.X + 64, e.Bounds.Y + 8 + (int)labelsize.Height + 4, (int)codesize.Height, (int)codesize.Width);
						if (cliprect.IntersectsWith(rect) == true)
						{
							e.Graphics.DrawString(code, e.Font, brush, new PointF(e.Bounds.X + 64, e.Bounds.Y + 8 + labelsize.Height + 4));
						}
					}

					rect = new Rectangle(e.Bounds.X + e.Bounds.Width - 56, e.Bounds.Y + 8, 48, 48);
					if (cliprect.IntersectsWith(rect) == true)
					{
						if (auth.AutoRefresh == true)
						{
							using (var piebrush = new SolidBrush(SystemColors.ActiveCaption))
							{
								using (var piepen = new Pen(SystemColors.ActiveCaption))
								{
									int tillUpdate = ((int)((auth.AuthenticatorData.ServerTime % 30000) / 1000L) + 1) * 12;
									e.Graphics.DrawPie(piepen, e.Bounds.X + e.Bounds.Width - 54, e.Bounds.Y + 10, 46, 46, 270, 360);
									e.Graphics.FillPie(piebrush, e.Bounds.X + e.Bounds.Width - 54, e.Bounds.Y + 10, 46, 46, 270, tillUpdate);
								}
							}
						}
						else
						{
							if (showCode == true)
							{
								using (var piebrush = new SolidBrush(SystemColors.ActiveCaption))
								{
									using (var piepen = new Pen(SystemColors.ActiveCaption))
									{
										int tillUpdate = (int)((item.DisplayUntil.Subtract(DateTime.Now).TotalSeconds * (double)360) / item.DisplayUntil.Subtract(item.LastUpdate).TotalSeconds);
										e.Graphics.DrawPie(piepen, e.Bounds.X + e.Bounds.Width - 54, e.Bounds.Y + 10, 46, 46, 270, 360);
										e.Graphics.FillPie(piebrush, e.Bounds.X + e.Bounds.Width - 54, e.Bounds.Y + 10, 46, 46, 270, tillUpdate);
									}
								}
							}
							else
							{
								e.Graphics.DrawImage(WinAuth.Properties.Resources.RefreshIcon, rect);
							}
						}
					}

					rect = new Rectangle(e.Bounds.X, e.Bounds.Y + this.ItemHeight - 1, 1, 1);
					if (cliprect.IntersectsWith(rect) == true)
					{
						using (Pen pen = new Pen(SystemColors.Control))
						{
							e.Graphics.DrawLine(pen, e.Bounds.X, e.Bounds.Y + this.ItemHeight - 1, e.Bounds.X + e.Bounds.Width, e.Bounds.Y + this.ItemHeight - 1);
						}
					}
				}
			}
		}

		//protected override void OnDrawItem(DrawItemEventArgs e)
		//{
		//	if (this.Items.Count > 0)
		//	{
		//		e.DrawBackground();
		//		OnDrawItem(e, new Rectangle(e.Bounds.X, e.Bounds.X, e.Bounds.Width, e.Bounds.Height));
		//	}
		//	base.OnDrawItem(e);
		//}

    protected override void OnPaint(PaintEventArgs e)
    {
      using (var brush = new SolidBrush(this.BackColor))
      {
        Region region = new Region(e.ClipRectangle);

        e.Graphics.FillRegion(brush, region);
        if (this.Items.Count > 0)
        {
          for (int i = 0; i < this.Items.Count; ++i)
          {
            Rectangle irect = this.GetItemRectangle(i);
            if (e.ClipRectangle.IntersectsWith(irect))
            {
							if ((this.SelectionMode == SelectionMode.One && this.SelectedIndex == i)
							|| (this.SelectionMode == SelectionMode.MultiSimple && this.SelectedIndices.Contains(i))
							|| (this.SelectionMode == SelectionMode.MultiExtended && this.SelectedIndices.Contains(i)))
							{
								DrawItemEventArgs diea = new DrawItemEventArgs(e.Graphics, this.Font,
										irect, i,
										DrawItemState.Selected, this.ForeColor,
										this.BackColor);
								OnDrawItem(diea, e.ClipRectangle);
								base.OnDrawItem(diea);
							}
							else
							{
								DrawItemEventArgs diea = new DrawItemEventArgs(e.Graphics, this.Font,
										irect, i,
										DrawItemState.Default, this.ForeColor,
										this.BackColor);
								OnDrawItem(diea, e.ClipRectangle);
								base.OnDrawItem(diea);
              }
              region.Complement(irect);
            }
          }
        }
      }

      base.OnPaint(e);
    }
  }
}
