// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Header.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Core.Mvc
{
    using Framework.Core.Extensions;
    #region

    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.Resources;

    #endregion

    /// <summary>
    ///     The label message.
    /// </summary>
    public class LabelMessage
    {
        /// <summary>
        ///     The messages key.
        /// </summary>
        protected const string MessagesKey = "HeaderMessages";
        private readonly IHttpContextAccessor _accessor;

        /// <summary>
        ///     The clear message default.
        /// </summary>
        private const bool ClearMessageDefault = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelMessage"/> class.
        /// </summary>
        /// <param name="resourceManager">
        /// The resource manager.
        /// </param>
        public LabelMessage(ResourceManager resourceManager, IHttpContextAccessor httpContextAccessor)
        {
            this.ResManager = resourceManager;
            _accessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets or sets the res manager.
        /// </summary>
        public ResourceManager ResManager { get; set; }

        /// <summary>
        ///     Gets or sets the messages.
        /// </summary>
        private List<MessageItem> Messages
        {
            get
            {
                var messages = _accessor.HttpContext.Items[MessagesKey] as List<MessageItem>;
                if (messages == null)
                {
                    messages = new List<MessageItem>();
                    _accessor.HttpContext.Items[MessagesKey] = messages;
                }

                return messages;
            }

            set => _accessor.HttpContext.Items[MessagesKey] = value;
        }

        /// <summary>
        ///     The clear messages.
        /// </summary>
        public void ClearMessages()
        {
            this.Messages = new List<MessageItem>();
        }

        /// <summary>
        /// The show error.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="dismissable">
        /// The dismissable.
        /// </param>
        /// <param name="clearMessages">
        /// The clear messages.
        /// </param>
        public void ShowError(string message, bool dismissable = false, bool clearMessages = true)
        {
            this.ShowMessage(
                message,
                AlertClasses.Danger,
                AlertIcons.Danger,
                null,
                null,
                null,
                dismissable,
                clearMessages);
        }

        /// <summary>
        /// The show error with link.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="urlLink">
        /// The url link.
        /// </param>
        /// <param name="urlLinkTitle">
        /// The url link title.
        /// </param>
        /// <param name="dismissable">
        /// The dismissable.
        /// </param>
        /// <param name="clearMessages">
        /// The clear messages.
        /// </param>
        public void ShowErrorWithLink(
            string message,
            string urlLink,
            string urlLinkTitle,
            bool dismissable = false,
            bool clearMessages = true)
        {
            this.ShowMessage(
                message,
                AlertClasses.Danger,
                AlertIcons.Danger,
                urlLink,
                urlLinkTitle,
                urlLinkTitle,
                dismissable,
                clearMessages);
        }

        /// <summary>
        /// The show info.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="dismissable">
        /// The dismissable.
        /// </param>
        /// <param name="clearMessages">
        /// The clear messages.
        /// </param>
        public void ShowInfo(string message, bool dismissable = false, bool clearMessages = true)
        {
            this.ShowMessage(
                message,
                AlertClasses.Information,
                AlertIcons.Information,
                null,
                null,
                null,
                dismissable,
                clearMessages);
        }

        /// <summary>
        /// The show info with link.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="urlLink">
        /// The url link.
        /// </param>
        /// <param name="urlLinkTitle">
        /// The url link title.
        /// </param>
        /// <param name="dismissable">
        /// The dismissable.
        /// </param>
        /// <param name="clearMessages">
        /// The clear messages.
        /// </param>
        public void ShowInfoWithLink(
            string message,
            string urlLink,
            string urlLinkTitle,
            bool dismissable = false,
            bool clearMessages = true)
        {
            this.ShowMessage(
                message,
                AlertClasses.Information,
                AlertIcons.Information,
                urlLink,
                urlLinkTitle,
                urlLinkTitle,
                dismissable,
                clearMessages);
        }

        /// <summary>
        ///     The show invalid parameters error.
        /// </summary>
        public void ShowInvalidParametersError()
        {
            this.ShowError(this.ResManager.GetString("InvalidParametersError"));
        }

        /// <summary>
        ///     The show item not found error.
        /// </summary>
        public void ShowItemNotFoundError()
        {
            this.ShowError(this.ResManager.GetString("ItemNotFoundError"));
        }

        /// <summary>
        /// The show success.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="dismissable">
        /// The dismissable.
        /// </param>
        /// <param name="clearMessages">
        /// The clear messages.
        /// </param>
        public void ShowSuccess(string message, bool dismissable = false, bool clearMessages = true)
        {
            this.ShowMessage(
                message,
                AlertClasses.Success,
                AlertIcons.Success,
                null,
                null,
                null,
                dismissable,
                clearMessages);
        }

        public void ShowSaveSuccess(string itemName, bool dismissable = false, bool clearMessages = true)
        {
            this.ShowSuccess(Resources.Messages.SaveSuccessFormat.StringFormat(itemName));
        }
        public void ShowEditSuccess(string itemName, bool dismissable = false, bool clearMessages = true)
        {
            this.ShowSuccess(Resources.Messages.EditSuccessFormat.StringFormat(itemName));
        }
        public void ShowDeleteSuccess(string itemName, bool dismissable = false, bool clearMessages = true)
        {
            this.ShowSuccess(Resources.Messages.DeleteSuccessFormat.StringFormat(itemName));
        }

        public void ShowCreateSuccess(string itemName, bool dismissable = false, bool clearMessages = true)
        {
            this.ShowSuccess(Resources.Messages.CreateSuccessFormat.StringFormat(itemName));
        }

        public void ShowAddSuccess(string itemName, bool dismissable = false, bool clearMessages = true)
        {
            this.ShowSuccess(Resources.Messages.AddSuccessFormat.StringFormat(itemName));
        }

        /// <summary>
        /// The show success with link.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="urlLink">
        /// The url link.
        /// </param>
        /// <param name="urlLinkTitle">
        /// The url link title.
        /// </param>
        /// <param name="dismissable">
        /// The dismissable.
        /// </param>
        /// <param name="clearMessages">
        /// The clear messages.
        /// </param>
        public void ShowSuccessWithLink(
            string message,
            string urlLink,
            string urlLinkTitle,
            bool dismissable = false,
            bool clearMessages = true)
        {
            this.ShowMessage(
                message,
                AlertClasses.Success,
                AlertIcons.Success,
                urlLink,
                urlLinkTitle,
                urlLinkTitle,
                dismissable,
                clearMessages);
        }

        /// <summary>
        ///     The show unauthorized access error.
        /// </summary>
        public void ShowUnauthorizedAccessError()
        {
            this.ShowError(this.ResManager.GetString("UnauthorizedAccess"));
        }

        /// <summary>
        ///     The show unexpected error.
        /// </summary>
        public void ShowUnexpectedError()
        {
            this.ShowError(this.ResManager.GetString("UnexpectedError"));
        }

        /// <summary>
        /// The show warning.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="dismissable">
        /// The dismissable.
        /// </param>
        /// <param name="clearMessages">
        /// The clear messages.
        /// </param>
        public void ShowWarning(string message, bool dismissable = false, bool clearMessages = true)
        {
            this.ShowMessage(
                message,
                AlertClasses.Warning,
                AlertIcons.Warning,
                null,
                null,
                null,
                dismissable,
                clearMessages);
        }

        /// <summary>
        /// The show warning with link.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="urlLink">
        /// The url link.
        /// </param>
        /// <param name="urlLinkTitle">
        /// The url link title.
        /// </param>
        /// <param name="dismissable">
        /// The dismissable.
        /// </param>
        /// <param name="clearMessages">
        /// The clear messages.
        /// </param>
        public void ShowWarningWithLink(
            string message,
            string urlLink,
            string urlLinkTitle,
            bool dismissable = false,
            bool clearMessages = true)
        {
            this.ShowMessage(
                message,
                AlertClasses.Warning,
                AlertIcons.Warning,
                urlLink,
                urlLinkTitle,
                urlLinkTitle,
                dismissable,
                clearMessages);
        }

        /// <summary>
        /// The show message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="alertStyle">
        /// The alert style.
        /// </param>
        /// <param name="alertIcon">
        /// The alert icon.
        /// </param>
        /// <param name="urlLink">
        /// The url link.
        /// </param>
        /// <param name="urlTitle">
        /// The url title.
        /// </param>
        /// <param name="urlLinkTitle">
        /// The url link title.
        /// </param>
        /// <param name="dismissable">
        /// The dismissable.
        /// </param>
        /// <param name="clearMessages">
        /// The clear messages.
        /// </param>
        private void ShowMessage(
            string message,
            string alertStyle,
            string alertIcon,
            string urlLink,
            string urlTitle,
            string urlLinkTitle,
            bool dismissable,
            bool clearMessages = false)
        {
            if (clearMessages)
            {
                this.Messages = new List<MessageItem>();
            }

            this.Messages.Add(
                new MessageItem(message, alertStyle, alertIcon, urlLink, urlTitle, urlLinkTitle, dismissable));
        }
    }

    /// <summary>
    ///     The message item.
    /// </summary>
    public class MessageItem
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageItem" /> class.
        /// </summary>
        public MessageItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageItem"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="alertCssClass">
        /// The alert css class.
        /// </param>
        /// <param name="alertIcon">
        /// The alert icon.
        /// </param>
        /// <param name="urlLink">
        /// The url link.
        /// </param>
        /// <param name="urlTitle">
        /// The url title.
        /// </param>
        /// <param name="urlLinkTitle">
        /// The url link title.
        /// </param>
        /// <param name="dismissable">
        /// The dismissable.
        /// </param>
        public MessageItem(
            string message,
            string alertCssClass,
            string alertIcon,
            string urlLink,
            string urlTitle,
            string urlLinkTitle,
            bool dismissable)
        {
            this.Message = message;
            this.AlertCssClass = alertCssClass;
            this.AlertIcon = alertIcon;
            this.UrlLink = urlLink;
            this.UrlTitle = urlTitle;
            this.UrlLinkTitle = urlLinkTitle;
            this.Dismissable = dismissable;
        }

        /// <summary>
        ///     Gets or sets the alert style.
        /// </summary>
        public string AlertCssClass { get; set; }

        /// <summary>
        ///     Gets or sets the alert icon.
        /// </summary>
        public string AlertIcon { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether dismissable.
        /// </summary>
        public bool Dismissable { get; set; }

        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     Gets or sets the url link.
        /// </summary>
        public string UrlLink { get; set; }

        /// <summary>
        ///     Gets or sets the url link title.
        /// </summary>
        public string UrlLinkTitle { get; set; }

        /// <summary>
        ///     Gets or sets the url title.
        /// </summary>
        public string UrlTitle { get; set; }
    }

    /// <summary>
    ///     The alert styles.
    /// </summary>
    public struct AlertClasses
    {
        /// <summary>
        ///     The danger.
        /// </summary>
        public const string Danger = "alert-danger";

        /// <summary>
        ///     The information.
        /// </summary>
        public const string Information = "alert-info";

        /// <summary>
        ///     The success.
        /// </summary>
        public const string Success = "alert-success";

        /// <summary>
        ///     The warning.
        /// </summary>
        public const string Warning = "alert-warning";
    }

    /// <summary>
    ///     The alert icons.
    /// </summary>
    public struct AlertIcons
    {
        /// <summary>
        ///     The danger icon.
        /// </summary>
        public const string Danger = "error_icon.png";

        /// <summary>
        ///     The information icon.
        /// </summary>
        public const string Information = "info_icon.png";

        /// <summary>
        ///     The success icon.
        /// </summary>
        public const string Success = "true_icon.png";

        /// <summary>
        ///     The warning icon.
        /// </summary>
        public const string Warning = "alert_icon.png";
    }
}