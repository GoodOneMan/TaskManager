﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TaskManagerUI.TMService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="UserStruct", Namespace="http://schemas.datacontract.org/2004/07/TaskManagerWCF_Lib.Struct")]
    [System.SerializableAttribute()]
    public partial class UserStruct : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string HostField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TaskManagerUI.TMService.TaskStruct[] TasksField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid UserGuidField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Host {
            get {
                return this.HostField;
            }
            set {
                if ((object.ReferenceEquals(this.HostField, value) != true)) {
                    this.HostField = value;
                    this.RaisePropertyChanged("Host");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TaskManagerUI.TMService.TaskStruct[] Tasks {
            get {
                return this.TasksField;
            }
            set {
                if ((object.ReferenceEquals(this.TasksField, value) != true)) {
                    this.TasksField = value;
                    this.RaisePropertyChanged("Tasks");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid UserGuid {
            get {
                return this.UserGuidField;
            }
            set {
                if ((this.UserGuidField.Equals(value) != true)) {
                    this.UserGuidField = value;
                    this.RaisePropertyChanged("UserGuid");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TaskStruct", Namespace="http://schemas.datacontract.org/2004/07/TaskManagerWCF_Lib.Struct")]
    [System.SerializableAttribute()]
    public partial class TaskStruct : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CommentField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] CommentCardField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid GuidTaskField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsCheckedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool StateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TitleField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UserField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Comment {
            get {
                return this.CommentField;
            }
            set {
                if ((object.ReferenceEquals(this.CommentField, value) != true)) {
                    this.CommentField = value;
                    this.RaisePropertyChanged("Comment");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] CommentCard {
            get {
                return this.CommentCardField;
            }
            set {
                if ((object.ReferenceEquals(this.CommentCardField, value) != true)) {
                    this.CommentCardField = value;
                    this.RaisePropertyChanged("CommentCard");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid GuidTask {
            get {
                return this.GuidTaskField;
            }
            set {
                if ((this.GuidTaskField.Equals(value) != true)) {
                    this.GuidTaskField = value;
                    this.RaisePropertyChanged("GuidTask");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsChecked {
            get {
                return this.IsCheckedField;
            }
            set {
                if ((this.IsCheckedField.Equals(value) != true)) {
                    this.IsCheckedField = value;
                    this.RaisePropertyChanged("IsChecked");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool State {
            get {
                return this.StateField;
            }
            set {
                if ((this.StateField.Equals(value) != true)) {
                    this.StateField = value;
                    this.RaisePropertyChanged("State");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Title {
            get {
                return this.TitleField;
            }
            set {
                if ((object.ReferenceEquals(this.TitleField, value) != true)) {
                    this.TitleField = value;
                    this.RaisePropertyChanged("Title");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string User {
            get {
                return this.UserField;
            }
            set {
                if ((object.ReferenceEquals(this.UserField, value) != true)) {
                    this.UserField = value;
                    this.RaisePropertyChanged("User");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="TMService.ITMService", CallbackContract=typeof(TaskManagerUI.TMService.ITMServiceCallback))]
    public interface ITMService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITMService/Connect", ReplyAction="http://tempuri.org/ITMService/ConnectResponse")]
        System.Guid Connect(TaskManagerUI.TMService.UserStruct user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITMService/Connect", ReplyAction="http://tempuri.org/ITMService/ConnectResponse")]
        System.Threading.Tasks.Task<System.Guid> ConnectAsync(TaskManagerUI.TMService.UserStruct user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITMService/Disconnect", ReplyAction="http://tempuri.org/ITMService/DisconnectResponse")]
        void Disconnect(System.Guid guid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITMService/Disconnect", ReplyAction="http://tempuri.org/ITMService/DisconnectResponse")]
        System.Threading.Tasks.Task DisconnectAsync(System.Guid guid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITMService/UpdataTasks", ReplyAction="http://tempuri.org/ITMService/UpdataTasksResponse")]
        void UpdataTasks(TaskManagerUI.TMService.TaskStruct task);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITMService/UpdataTasks", ReplyAction="http://tempuri.org/ITMService/UpdataTasksResponse")]
        System.Threading.Tasks.Task UpdataTasksAsync(TaskManagerUI.TMService.TaskStruct task);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ITMServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/ITMService/NotifyUpdataTasksCallback")]
        void NotifyUpdataTasksCallback(TaskManagerUI.TMService.TaskStruct task);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ITMServiceChannel : TaskManagerUI.TMService.ITMService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class TMServiceClient : System.ServiceModel.DuplexClientBase<TaskManagerUI.TMService.ITMService>, TaskManagerUI.TMService.ITMService {
        
        public TMServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public TMServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public TMServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public TMServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public TMServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public System.Guid Connect(TaskManagerUI.TMService.UserStruct user) {
            return base.Channel.Connect(user);
        }
        
        public System.Threading.Tasks.Task<System.Guid> ConnectAsync(TaskManagerUI.TMService.UserStruct user) {
            return base.Channel.ConnectAsync(user);
        }
        
        public void Disconnect(System.Guid guid) {
            base.Channel.Disconnect(guid);
        }
        
        public System.Threading.Tasks.Task DisconnectAsync(System.Guid guid) {
            return base.Channel.DisconnectAsync(guid);
        }
        
        public void UpdataTasks(TaskManagerUI.TMService.TaskStruct task) {
            base.Channel.UpdataTasks(task);
        }
        
        public System.Threading.Tasks.Task UpdataTasksAsync(TaskManagerUI.TMService.TaskStruct task) {
            return base.Channel.UpdataTasksAsync(task);
        }
    }
}