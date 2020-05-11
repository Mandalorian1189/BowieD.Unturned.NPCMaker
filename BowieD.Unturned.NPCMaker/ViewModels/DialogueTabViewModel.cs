﻿using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BowieD.Unturned.NPCMaker.ViewModels
{
    public sealed class DialogueTabViewModel : BaseViewModel
    {
        private NPCDialogue _dialogue;
        public DialogueTabViewModel()
        {
            Dialogue = new NPCDialogue();
        }
        public NPCDialogue Dialogue
        {
            get => _dialogue;
            set
            {
                _dialogue = value;
                UpdateMessages();
                UpdateResponses();
                OnPropertyChange("");
            }
        }
        public ushort ID
        {
            get => Dialogue.id;
            set
            {
                Dialogue.id = value;
                OnPropertyChange("ID");
            }
        }
        public string Comment
        {
            get => Dialogue.Comment;
            set
            {
                Dialogue.Comment = value;
                OnPropertyChange("Comment");
            }
        }
        public List<NPCMessage> Messages
        {
            get
            {
                List<Dialogue_Message> messages = new List<Dialogue_Message>();
                foreach (UIElement ui in MainWindow.Instance.messagePagesGrid.Children)
                {
                    if (ui is Dialogue_Message dm)
                    {
                        messages.Add(dm);
                    }
                }
                return messages.Select(d => d.Message).ToList();
            }
            set
            {
                Dialogue.messages = value;
                UpdateMessages();
                OnPropertyChange("Messages");
            }
        }
        public List<NPCResponse> Responses
        {
            get
            {
                List<Dialogue_Response> responses = new List<Dialogue_Response>();
                foreach (UIElement ui in MainWindow.Instance.dialoguePlayerRepliesGrid.Children)
                {
                    if (ui is Dialogue_Response dr)
                    {
                        responses.Add(dr);
                    }
                }
                return responses.Select(d => d.Response).ToList();
            }
            set
            {
                Dialogue.responses = value;
                UpdateResponses();
                OnPropertyChange("Responses");
            }
        }

        private ICommand saveCommand, openCommand, resetCommand, addReplyCommand, addMessageCommand, setAsStartCommand, previewCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new BaseCommand(() =>
                    {
                        if (ID == 0)
                        {
                            App.NotificationManager.Notify(LocalizationManager.Current.Notification["Dialogue_ID_Zero"]);
                            return;
                        }
                        Dialogue.messages = Messages;
                        Dialogue.responses = Responses;
                        if (!MainWindow.CurrentProject.data.dialogues.Contains(Dialogue))
                        {
                            MainWindow.CurrentProject.data.dialogues.Add(Dialogue);
                        }

                        App.NotificationManager.Notify(LocalizationManager.Current.Notification["Dialogue_Saved"]);
                        MainWindow.CurrentProject.isSaved = false;
                        App.Logger.Log($"Dialogue {ID} saved!");
                    });
                }
                return saveCommand;
            }
        }
        public ICommand OpenCommand
        {
            get
            {
                if (openCommand == null)
                {
                    openCommand = new BaseCommand(() =>
                    {
                        Universal_ListView ulv = new Universal_ListView(MainWindow.CurrentProject.data.dialogues.OrderBy(d => d.id).Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Dialogue, false)).ToList(), Universal_ItemList.ReturnType.Dialogue);
                        if (ulv.ShowDialog() == true)
                        {
                            SaveCommand.Execute(null);
                            Dialogue = ulv.SelectedValue as NPCDialogue;
                            App.Logger.Log($"Opened dialogue {ID}");
                        }
                        MainWindow.CurrentProject.data.dialogues = ulv.Values.Cast<NPCDialogue>().ToList();
                    });
                }
                return openCommand;
            }
        }
        public ICommand ResetCommand
        {
            get
            {
                if (resetCommand == null)
                {
                    resetCommand = new BaseCommand(() =>
                    {
                        ushort id = ID;
                        Dialogue = new NPCDialogue();
                        App.Logger.Log($"Cleared dialogue {id}");
                    });
                }
                return resetCommand;
            }
        }
        public ICommand AddReplyCommand
        {
            get
            {
                if (addReplyCommand == null)
                {
                    addReplyCommand = new BaseCommand(() =>
                    {
                        Dialogue.responses.Add(new NPCResponse());
                        UpdateResponses();
                    });
                }
                return addReplyCommand;
            }
        }
        public ICommand AddMessageCommand
        {
            get
            {
                if (addMessageCommand == null)
                {
                    addMessageCommand = new BaseCommand(() =>
                    {
                        Dialogue.messages = Messages;
                        Dialogue.messages.Add(new NPCMessage());
                        UpdateMessages();
                    });
                }
                return addMessageCommand;
            }
        }
        public ICommand SetAsStartCommand
        {
            get
            {
                if (setAsStartCommand == null)
                {
                    setAsStartCommand = new BaseCommand(() =>
                    {
                        SaveCommand.Execute(null);
                        if (ID > 0 && MainWindow.Instance.MainWindowViewModel.CharacterTabViewModel.DialogueID != ID)
                        {
                            MainWindow.Instance.MainWindowViewModel.CharacterTabViewModel.DialogueID = ID;
                            App.NotificationManager.Notify(LocalizationManager.Current.Notification.Translate("Dialogue_SetAsStart", ID));
                            App.Logger.Log($"Dialogue {ID} set as start!");
                        }
                    });
                }
                return setAsStartCommand;
            }
        }
        public ICommand PreviewCommand
        {
            get
            {
                if (previewCommand == null)
                {
                    previewCommand = new BaseCommand(() =>
                    {
                        SaveCommand.Execute(null);
                        Universal_Select select = new Universal_Select(Universal_ItemList.ReturnType.Character);
                        if (select.ShowDialog() == true)
                        {
                            NPCCharacter character = select.SelectedValue as NPCCharacter;

                            DialogueView_Window dvw = new DialogueView_Window(character, Dialogue, new Simulation());
                            dvw.Display();
                            dvw.ShowDialog();
                        }
                    });
                }
                return previewCommand;
            }
        }
        public void UpdateMessages()
        {
            List<UIElement> toRemove = new List<UIElement>();
            foreach (UIElement item in MainWindow.Instance.messagePagesGrid.Children)
            {
                if (item is Dialogue_Message)
                {
                    toRemove.Add(item);
                }
            }
            foreach (UIElement item in toRemove)
            {
                MainWindow.Instance.messagePagesGrid.Children.Remove(item);
            }
            foreach (NPCMessage msg in Dialogue.messages)
            {
                Dialogue_Message dialogue_Message = new Dialogue_Message(msg);
                dialogue_Message.deletePageButton.Click += (sender, e) =>
                {
                    Dialogue.messages.Remove(msg);
                    UpdateMessages();
                };
                int ind = 0;
                for (int k = 0; k < MainWindow.Instance.messagePagesGrid.Children.Count; k++)
                {
                    if (MainWindow.Instance.messagePagesGrid.Children[k] is Dialogue_Message)
                    {
                        ind = k + 1;
                    }
                }
                MainWindow.Instance.messagePagesGrid.Children.Insert(ind, dialogue_Message);
            }
        }
        public void UpdateResponses()
        {
            List<UIElement> toRemove = new List<UIElement>();
            foreach (UIElement item in MainWindow.Instance.dialoguePlayerRepliesGrid.Children)
            {
                if (item is Dialogue_Response)
                {
                    toRemove.Add(item);
                }
            }
            foreach (UIElement item in toRemove)
            {
                MainWindow.Instance.dialoguePlayerRepliesGrid.Children.Remove(item);
            }
            foreach (NPCResponse res in Dialogue.responses)
            {
                Dialogue_Response dialogue_Response = new Dialogue_Response(res);
                dialogue_Response.deleteButton.Click += (sender, e) =>
                {
                    Dialogue.responses.Remove(res);
                    UpdateResponses();
                };
                int ind = 0;
                for (int k = 0; k < MainWindow.Instance.dialoguePlayerRepliesGrid.Children.Count; k++)
                {
                    if (MainWindow.Instance.dialoguePlayerRepliesGrid.Children[k] is Dialogue_Response)
                    {
                        ind = k + 1;
                    }
                }
                MainWindow.Instance.dialoguePlayerRepliesGrid.Children.Insert(ind, dialogue_Response);
            }
            foreach (object res in MainWindow.Instance.dialoguePlayerRepliesGrid.Children)
            {
                if (res is Dialogue_Response dr)
                {
                    dr.UpdateOrderButtons();
                }
            }
        }
    }
}
