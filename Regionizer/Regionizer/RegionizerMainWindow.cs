﻿

#region using statements

using DataJuggler.Core.UltimateHelper;
using DataJuggler.Regionizer.CodeModel.Objects;
using DataJuggler.Regionizer.Controls;
using DataJuggler.Regionizer.Parsers;
using DataJuggler.Regionizer.Controls.Util;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using XmlMirror.Runtime.Util;
using DataJuggler.Regionizer.UI.Forms;

#endregion

namespace DataJuggler.Regionizer
{

    #region class RegionizerMainWindow : ToolWindowPane
    /// <summary>
    /// This is the MainWindow for the Regionizer Package. 
    /// This object is used to format code into regions.
    /// </summary>
    public class RegionizerMainWindow : ToolWindowPane
    {
        
        #region Private Variables
        private CommentDictionary commentDictionairy;
        private IServiceProvider serviceProvider;
        #endregion
        
        #region Constructor
        /// <summary>
        /// Standard constructor for the tool window.
        /// </summary>
        public RegionizerMainWindow() : base(null)
        {
            // set the service provider
            this.ServiceProvider = this;

            // Set the window title
            this.Caption = "Regionizer";

            // Set the image that will appear on the tab of the window frame
            // when docked with an other window
            // The resource ID correspond to the one defined in the resx file
            // while the Index is the offset in the bitmap strip. Each image in
            // the strip being 16x16.
            this.BitmapResourceID = 301;
            this.BitmapIndex = 1;
            
            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on 
            // the object returned by the Content property.
            RegionizerMainWindowControl  regionizerToolWindow = new RegionizerMainWindowControl();
            regionizerToolWindow.HostEventHandler = HostEventListener;
            base.Content = regionizerToolWindow;
        }
        #endregion
        
        #region Events
            
            #region HostEventListener(string eventName, object args)
            /// <summary>
            /// This method listens for command comming in.
            /// </summary>
            public void HostEventListener(string eventName, object args)
            {
                // locals
                EnvDTE.DTE dte = null;
                string dataType = "";
                RegionizerCodeManager codeManager = null;
                CSharpCodeFile codeFile = null;
                string dictionaryPath = "";
                DictionaryInfo dictionaryInfo = null;

                try
                {
                    // get dte
                    dte = (EnvDTE.DTE)GetService(typeof(EnvDTE.DTE));
                    
                    switch (eventName)
                    {
                        case "Format Document":
                        
                            // if the dte object exists
                            if ((dte != null) && (dte.ActiveDocument != null))
                            {
                                //// Create the code manager object
                                codeManager = new RegionizerCodeManager(dte.ActiveDocument);
                                codeManager.FormatDocument();
                            }
                        
                            // required
                            break;
                        
                        case "InsertPrivateVariable":
                        
                            // if the dte object exists
                            if ((dte != null) && (dte.ActiveDocument != null))
                            {
                                // Create the code manager object
                                codeManager = new RegionizerCodeManager(dte.ActiveDocument);
                                string privateVariableText = args.ToString();
                            
                                // Get the code file
                                codeFile = codeManager.GetCodeFile();
                            
                                // insert the private variable text
                                codeManager.InsertPrivateVariable(privateVariableText, codeFile);
                            }
                        
                            // required
                            break;
                        
                        case "CreatePropertiesFromSelection":
                        
                            // if the dte object exists
                            if ((dte != null) && (dte.ActiveDocument != null))
                            {
                                // Create the code manager object
                                codeManager = new RegionizerCodeManager(dte.ActiveDocument);
                                string privateVariableText = codeManager.GetSelectedText(true);
                            
                                // insert the private variable text
                                codeManager.InsertPropertiesFromSelectedText(privateVariableText, codeManager);
                            }
                        
                            // required
                            break;
                        
                        case "CreateHasPropertyFromSelection":
                        
                            // if the dte object exists
                            if ((dte != null) && (dte.ActiveDocument != null))
                            {
                                // Create the code manager object
                                codeManager = new RegionizerCodeManager(dte.ActiveDocument);
                                string propertyName = codeManager.GetSelectedText(ref dataType);
                            
                                // Get the code file
                                codeFile = codeManager.GetCodeFile();
                            
                                // insert the private variable text
                                codeManager.InsertHasProperty(propertyName, dataType, codeFile);
                            }
                        
                            // required
                            break;
                        
                        case "InsertMethod":
                        
                            // if the dte object exists
                            if ((dte != null) && (dte.ActiveDocument != null))
                            {
                                // Create the code manager object
                                codeManager = new RegionizerCodeManager(dte.ActiveDocument);
                            
                                // Get the code file
                                codeFile = codeManager.GetCodeFile();
                            
                                // get the args
                                string methodName = "";
                                string returnType = "";
                            
                                // if there are one or more args
                                if (args != null)
                                {
                                    // cast the args as a string
                                    IList<string> stringArgs = (List<string>) args;
                                
                                    // if there are two args
                                    if ((stringArgs != null) && (stringArgs.Count == 2))
                                    {
                                        // set the values
                                        methodName = stringArgs[0];
                                        returnType = stringArgs[1];
                                    }
                                }
                            
                                // insert the private variable text
                                codeManager.InsertMethod(methodName, returnType, codeFile);
                            }
                        
                            // required
                            break;
                        
                        case "InsertEvent":
                        
                            // if the dte object exists
                            if ((dte != null) && (dte.ActiveDocument != null))
                            {
                                // Create the code manager object
                                codeManager = new RegionizerCodeManager(dte.ActiveDocument);
                            
                                // Get the code file
                                codeFile = codeManager.GetCodeFile();
                            
                                // get the args
                                string eventName2 = "";
                                string returnType = "";
                            
                                // if the args exist
                                if (args != null)
                                {
                                    // cast the args as a string
                                    IList<string> stringArgs = (List<string>)args;
                                
                                    // if there are two args
                                    if ((stringArgs != null) && (stringArgs.Count == 2))
                                    {
                                        // set the values
                                        eventName2 = stringArgs[0];
                                        returnType = stringArgs[1];
                                    }
                                }
                            
                                // insert the private variable text
                                codeManager.InsertEvent(eventName2, returnType, codeFile);
                            }
                        
                            // required
                            break;
                        
                        case "CollapseAllRegions":

                            //// Create the code manager object
                            //codeManager = new RegionizerCodeManager(dte.ActiveDocument);

                            //// Get the code file
                            //codeFile = codeManager.GetCodeFile();

                            //// collapse all regions
                            //codeManager.CollapseAllRegions(codeFile, dte);
                        
                            //// required
                            break;
                        
                        case "Format Selection":

                            // if the dte object exists
                            if ((dte != null) && (dte.ActiveDocument != null))
                            {
                                // Create the code manager object
                                codeManager = new RegionizerCodeManager(dte.ActiveDocument);

                                // Format the selection
                                codeManager.FormatSelection();
                            }
                        
                            // required
                            break;

                        case "InitialValueButton_Click":

                            // if the dte object exists
                            if ((dte != null) && (dte.ActiveDocument != null))
                            {
                                // Create the code manager object
                                codeManager = new RegionizerCodeManager(dte.ActiveDocument);

                                // set the return type
                                string completeArgs = args.ToString();
                                char[] seperators = new char[]{'|'};
                                string[] words = completeArgs.Split(seperators);
                                
                                // set the return type and variable and name
                                string returnType = "";
                                string variableName = "";
                                
                                // if there are two words
                                if ((words != null) && (words.Length == 2))
                                {
                                    returnType = words[0];
                                    variableName = words[1];
                                }

                                // Format the selection
                                codeManager.InsertInitialValue(returnType, variableName);
                            }

                            // required
                            break;

                        case "IfExistsButton_Click":

                            // if the dte object exists
                            if ((dte != null) && (dte.ActiveDocument != null))
                            {
                                // Create the code manager object
                                codeManager = new RegionizerCodeManager(dte.ActiveDocument);

                                // set the return type
                                string completeArgs = args.ToString();
                                char[] seperators = new char[] { '|' };
                                string[] words = completeArgs.Split(seperators);

                                // set the return type and variable and name
                                string commentLine = "";
                                string testLine = "";

                                // if there are two words
                                if ((words != null) && (words.Length == 2))
                                {
                                    commentLine = words[0];
                                    testLine = words[1];
                                }

                                // Format the selection
                                codeManager.InsertIfExistsStatement(commentLine, testLine);
                            }

                            // required
                            break;

                        case "ForEachButton_Click":

                            // if the dte object exists
                            if ((dte != null) && (dte.ActiveDocument != null))
                            {
                                // Create the code manager object
                                codeManager = new RegionizerCodeManager(dte.ActiveDocument);

                                // set the return type
                                string completeArgs = args.ToString();
                                char[] seperators = new char[] { '|' };
                                string[] words = completeArgs.Split(seperators);

                                // set the return type and variable and name
                                string returnType = "";
                                string collectionName = "";

                                // if there are two words
                                if ((words != null) && (words.Length == 2))
                                {
                                    returnType = words[0];
                                    collectionName = words[1];
                                }

                                // Format the selection
                                codeManager.InsertForEachLoop(returnType, collectionName);
                            }

                            // required
                            break;

                        case "LaunchXmlReservedCharacterHelperForm":

                            // Create an instance of the XmlReservedCharacterHelperForm
                            XmlReservedCharacterHelperForm xmlReservedCharacterHelperForm = new XmlReservedCharacterHelperForm();

                            // Show the form
                            xmlReservedCharacterHelperForm.ShowDialog();

                            // required
                            break;

                        case "LoadCommentDictionary":

                            // Load the CommentDictionary
                            LoadCommentDictionary();

                            // required
                            break;

                        case "AutoComment":

                            // if the CommentDictionary exists
                            if (this.HasCommentDictionairy)
                            {
                                // get the dictionaryInfo
                                dictionaryInfo = RegistryHelper.GetDictionaryInfo();

                                // if the dte object exists
                                if ((dte != null) && (dte.ActiveDocument != null) && (dictionaryInfo != null))
                                {
                                    // Create the code manager object
                                    codeManager = new RegionizerCodeManager(dte.ActiveDocument);

                                    // set the CommentDicationary
                                    codeManager.CommentDictionary = this.CommentDictionairy;

                                    // Auto Comment The Line Below The Current Selection
                                    codeManager.AutoComment(dictionaryInfo);
                                }
                            }

                            // required
                            break;

                        case "EditCommentDictionary":

                            // set the dictionaryPath
                            dictionaryPath = args.ToString();

                            // open the document
                            OpenDocumentInNewWindow(dictionaryPath);

                            // required
                            break;

                        case "SetupCommentDictionary":

                            // create an instance of a setupForm
                            CommentDictionarySetupForm setupForm = new CommentDictionarySetupForm();

                            // Set the DictionaryInfo
                            dictionaryInfo = RegistryHelper.GetDictionaryInfo();
                            
                            // call the setup method on the CommentDictionarySetupForm
                            setupForm.Setup(dictionaryInfo);

                            // show the setupForm
                            setupForm.ShowDialog();
                            
                            // required
                            break;
                    }
                }
                catch (Exception error)
                {
                    string err = error.ToString();
                    
                    // Show the error
                    MessageBox.Show(err, "File Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            #endregion
            
        #endregion

        #region Methods
            
            #region LoadCommentDictionary()
            /// <summary>
            /// This method Loads the CommentDictionary
            /// </summary>
            private void LoadCommentDictionary()
            {
                // locals
                string dictionaryPath = "";
                string commentDictionaryText = "";
                string customDictionaryText = "";
                CommentDictionaryParser parser = null;

                try 
	            {
                    // destory the CommentDictionary in case it exists
                    this.CommentDictionairy = null;

                    // load the dictionaryInfo
                    DictionaryInfo dictionaryInfo = RegistryHelper.GetDictionaryInfo();
                      
                    // if the dictionaryInfo object exists               
		            if (dictionaryInfo != null)
                    {
                        // get the path to the dictionairy
                        dictionaryPath = dictionaryInfo.DictionaryPath;
                    }

                    // if the dictionaryPath exists
                    if (!String.IsNullOrEmpty(dictionaryPath))
                    {
                        // read all the text
                        commentDictionaryText = File.ReadAllText(dictionaryPath);
                    }

                    // if the commentDictionairyText was loaded
                    if (TextHelper.Exists(commentDictionaryText))
                    {  
                        // create the CommentDictionary
                        this.CommentDictionairy = new CommentDictionary();

                        // Create the CommentDictionaryParser
                        parser = new CommentDictionaryParser();

                        // load the comments
                        this.CommentDictionairy.Comments = parser.LoadCodeComments(commentDictionaryText);
                    }

                    // if useCustomDictionary is true and the CustomDictionaryPath exists
                    if ((dictionaryInfo.UseCustomDictionary) && (TextHelper.Exists(dictionaryInfo.CustomDictionaryPath)))
                    {
                        // load the text
                        customDictionaryText = File.ReadAllText(dictionaryInfo.CustomDictionaryPath);
                    }

                    // if the customDictionaryText was loaded
                    if (TextHelper.Exists(customDictionaryText))
                    {
                        // Create the CommentDictionaryParser
                        parser = new CommentDictionaryParser();

                        // load the comments
                        this.CommentDictionairy.CustomComments = parser.LoadCodeComments(customDictionaryText);
                    }

                    // Set CommentsLoaded to true
                    this.CommentDictionairy.CommentsLoaded = true;
	            }
	            catch (Exception error)
	            {
                    // for debugging only
                    string err = error.ToString();

                    // Show the user an error
                    MessageBox.Show("An error occurred attempted to load the CommentDictionary.");
	            }
            }
            #endregion

            #region OpenDocumentInNewWindow(string filePath)
            /// <summary>
            /// This method opens a file in the editor window
            /// </summary>
            /// <param name="filePath"></param>
            /// <returns></returns>
            public IVsWindowFrame OpenDocumentInNewWindow(string filePath)
            {
                // initial value
                IVsWindowFrame frame = null;

                // locals
                IVsUIHierarchy hierarchy;
                uint itemId;

                // if the ServiceProvider exists
                if (this.HasServiceProvider)
                {
                    // if the filePath exists
                    if (TextHelper.Exists(filePath))
                    {
                        // if the Document is not open
                        if (!VsShellUtilities.IsDocumentOpen(this.ServiceProvider, filePath, VSConstants.LOGVIEWID_Primary, out hierarchy, out itemId, out frame))
                        {
                            // open the document
                            VsShellUtilities.OpenDocument(this.ServiceProvider, filePath, VSConstants.LOGVIEWID_Primary, out hierarchy, out itemId, out frame);
                        }
                    }
                }

                // if the frame exists
                if (frame != null)
                {
                    // show the frame
                    frame.Show();
                }

                // return value
                return frame;
            }
            #endregion
            
        #endregion

        #region Properties

            #region CommentDictionary
            /// <summary>
            /// This property gets or sets the value for 'CommentDictionary'.
            /// </summary>
            public CommentDictionary CommentDictionairy
            {
                get { return commentDictionairy; }
                set { commentDictionairy = value; }
            }
            #endregion
            
            #region HasCommentDictionary
            /// <summary>
            /// This property returns true if this object has a 'CommentDictionary'.
            /// </summary>
            public bool HasCommentDictionairy
            {
                get
                {
                    // initial value
                    bool hasCommentDictionairy = (this.CommentDictionairy != null);
                    
                    // return value
                    return hasCommentDictionairy;
                }
            }
            #endregion
            
            #region HasServiceProvider
            /// <summary>
            /// This property returns true if this object has a 'ServiceProvider'.
            /// </summary>
            public bool HasServiceProvider
            {
                get
                {
                    // initial value
                    bool hasServiceProvider = (this.ServiceProvider != null);
                    
                    // return value
                    return hasServiceProvider;
                }
            }
            #endregion
            
            #region ServiceProvider
            /// <summary>
            /// This property gets or sets the value for 'ServiceProvider'.
            /// </summary>
            public IServiceProvider ServiceProvider
            {
                get { return serviceProvider; }
                set { serviceProvider = value; }
            }
            #endregion
            
        #endregion
        
    }
    #endregion

}
