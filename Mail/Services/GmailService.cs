﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using CommunityToolkit.Authentication;
using Mail.Models;
using Mail.Services.Data;
using Mail.Services.Data.Enums;

namespace Mail.Services
{
    abstract class GmailService : OAuthMailService
    {
        protected override string[] Scopes { get; } = new string[] { "" };

        public GmailService() : base(WebAccountProviderType.Any)
        {
        }

        public override MailType MailType => MailType.Gmail;

        public override IAsyncEnumerable<MailFolderData> GetMailSuperFoldersAsync(NavigationMode NavigationMode,
            CancellationToken CancelToken = default)
        {
            throw new System.NotImplementedException();
        }

        public override Task<MailFolderDetailData> GetMailFolderDetailAsync(string RootFolderId,
            CancellationToken CancelToken = default)
        {
            throw new System.NotImplementedException();
        }

        public override Task<MailFolderDetailData> GetMailFolderDetailAsync(MailFolderType Type,
            CancellationToken CancelToken = default)
        {
            throw new System.NotImplementedException();
        }

        public override IAsyncEnumerable<MailMessageData> GetMailMessageAsync(string RootFolderId, uint StartIndex = 0,
            uint Count = 30, CancellationToken CancelToken = default)
        {
            throw new System.NotImplementedException();
        }

        public override IAsyncEnumerable<MailMessageData> GetMailMessageAsync(MailFolderType Type, uint StartIndex = 0,
            uint Count = 30, CancellationToken CancelToken = default)
        {
            throw new System.NotImplementedException();
        }

        public override Task<IReadOnlyList<ContactModel>> GetContactsAsync(CancellationToken CancelToken = default)
        {
            throw new System.NotImplementedException();
        }

        public override IAsyncEnumerable<MailMessageFileAttachmentData> GetMailAttachmentFileAsync(
            MailMessageListDetailViewModel model, CancellationToken cancelToken)
        {
            throw new System.NotImplementedException();
        }

        public override Task LoadAttachmentsAndCacheAsync(string messageId, CancellationToken cancelToken)
        {
            throw new System.NotImplementedException();
        }

        public override Task<bool> MailDraftSaveAsync(MailMessageListDetailViewModel Model)
        {
            throw new System.NotImplementedException();
        }

        public override Task<bool> MailSendAsync(MailMessageListDetailViewModel Model)
        {
            throw new System.NotImplementedException();
        }

        public override Task<bool> MailReplyAsync(MailMessageListDetailViewModel Model, string ReplyContent, bool IsAll)
        {
            throw new System.NotImplementedException();
        }

        public override Task<bool> MailForwardAsync(MailMessageListDetailViewModel Model, string ForwardContent)
        {
            throw new System.NotImplementedException();
        }

        public override Task<bool> MailMoveAsync(string mailMessageId, string folderId)
        {
            throw new System.NotImplementedException();
        }

        public override Task<bool> MailRemoveAsync(MailMessageListDetailViewModel Model)
        {
            throw new System.NotImplementedException();
        }
    }
}