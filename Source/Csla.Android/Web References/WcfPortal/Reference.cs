//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.1.
// 
#pragma warning disable 1591

namespace Csla.WcfPortal {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="BasicHttpBinding_IWcfPortal", Namespace="http://tempuri.org/")]
    public partial class WcfPortal : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback CreateOperationCompleted;
        
        private System.Threading.SendOrPostCallback FetchOperationCompleted;
        
        private System.Threading.SendOrPostCallback UpdateOperationCompleted;
        
        private System.Threading.SendOrPostCallback DeleteOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WcfPortal() {
            this.Url = "http://localhost:21647/SlPortal.svc";
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event CreateCompletedEventHandler CreateCompleted;
        
        /// <remarks/>
        public event FetchCompletedEventHandler FetchCompleted;
        
        /// <remarks/>
        public event UpdateCompletedEventHandler UpdateCompleted;
        
        /// <remarks/>
        public event DeleteCompletedEventHandler DeleteCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ws.lhotka.net/WcfDataPortal/IWcfPortal/Create", RequestNamespace="http://ws.lhotka.net/WcfDataPortal", ResponseNamespace="http://ws.lhotka.net/WcfDataPortal", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public WcfResponse Create([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] CriteriaRequest request) {
            object[] results = this.Invoke("Create", new object[] {
                        request});
            return ((WcfResponse)(results[0]));
        }
        
        /// <remarks/>
        public void CreateAsync(CriteriaRequest request) {
            this.CreateAsync(request, null);
        }
        
        /// <remarks/>
        public void CreateAsync(CriteriaRequest request, object userState) {
            if ((this.CreateOperationCompleted == null)) {
                this.CreateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreateOperationCompleted);
            }
            this.InvokeAsync("Create", new object[] {
                        request}, this.CreateOperationCompleted, userState);
        }
        
        private void OnCreateOperationCompleted(object arg) {
            if ((this.CreateCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CreateCompleted(this, new CreateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ws.lhotka.net/WcfDataPortal/IWcfPortal/Fetch", RequestNamespace="http://ws.lhotka.net/WcfDataPortal", ResponseNamespace="http://ws.lhotka.net/WcfDataPortal", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public WcfResponse Fetch([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] CriteriaRequest request) {
            object[] results = this.Invoke("Fetch", new object[] {
                        request});
            return ((WcfResponse)(results[0]));
        }
        
        /// <remarks/>
        public void FetchAsync(CriteriaRequest request) {
            this.FetchAsync(request, null);
        }
        
        /// <remarks/>
        public void FetchAsync(CriteriaRequest request, object userState) {
            if ((this.FetchOperationCompleted == null)) {
                this.FetchOperationCompleted = new System.Threading.SendOrPostCallback(this.OnFetchOperationCompleted);
            }
            this.InvokeAsync("Fetch", new object[] {
                        request}, this.FetchOperationCompleted, userState);
        }
        
        private void OnFetchOperationCompleted(object arg) {
            if ((this.FetchCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.FetchCompleted(this, new FetchCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ws.lhotka.net/WcfDataPortal/IWcfPortal/Update", RequestNamespace="http://ws.lhotka.net/WcfDataPortal", ResponseNamespace="http://ws.lhotka.net/WcfDataPortal", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public WcfResponse Update([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] UpdateRequest request) {
            object[] results = this.Invoke("Update", new object[] {
                        request});
            return ((WcfResponse)(results[0]));
        }
        
        /// <remarks/>
        public void UpdateAsync(UpdateRequest request) {
            this.UpdateAsync(request, null);
        }
        
        /// <remarks/>
        public void UpdateAsync(UpdateRequest request, object userState) {
            if ((this.UpdateOperationCompleted == null)) {
                this.UpdateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateOperationCompleted);
            }
            this.InvokeAsync("Update", new object[] {
                        request}, this.UpdateOperationCompleted, userState);
        }
        
        private void OnUpdateOperationCompleted(object arg) {
            if ((this.UpdateCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UpdateCompleted(this, new UpdateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ws.lhotka.net/WcfDataPortal/IWcfPortal/Delete", RequestNamespace="http://ws.lhotka.net/WcfDataPortal", ResponseNamespace="http://ws.lhotka.net/WcfDataPortal", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public WcfResponse Delete([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] CriteriaRequest request) {
            object[] results = this.Invoke("Delete", new object[] {
                        request});
            return ((WcfResponse)(results[0]));
        }
        
        /// <remarks/>
        public void DeleteAsync(CriteriaRequest request) {
            this.DeleteAsync(request, null);
        }
        
        /// <remarks/>
        public void DeleteAsync(CriteriaRequest request, object userState) {
            if ((this.DeleteOperationCompleted == null)) {
                this.DeleteOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteOperationCompleted);
            }
            this.InvokeAsync("Delete", new object[] {
                        request}, this.DeleteOperationCompleted, userState);
        }
        
        private void OnDeleteOperationCompleted(object arg) {
            if ((this.DeleteCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteCompleted(this, new DeleteCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/Csla.Server.Hosts.Silverlight")]
    public partial class CriteriaRequest {
        
        private byte[] clientContextField;
        
        private string clientCultureField;
        
        private string clientUICultureField;
        
        private byte[] criteriaDataField;
        
        private byte[] globalContextField;
        
        private byte[] principalField;
        
        private string typeNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", IsNullable=true)]
        public byte[] ClientContext {
            get {
                return this.clientContextField;
            }
            set {
                this.clientContextField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ClientCulture {
            get {
                return this.clientCultureField;
            }
            set {
                this.clientCultureField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ClientUICulture {
            get {
                return this.clientUICultureField;
            }
            set {
                this.clientUICultureField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", IsNullable=true)]
        public byte[] CriteriaData {
            get {
                return this.criteriaDataField;
            }
            set {
                this.criteriaDataField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", IsNullable=true)]
        public byte[] GlobalContext {
            get {
                return this.globalContextField;
            }
            set {
                this.globalContextField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", IsNullable=true)]
        public byte[] Principal {
            get {
                return this.principalField;
            }
            set {
                this.principalField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string TypeName {
            get {
                return this.typeNameField;
            }
            set {
                this.typeNameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/Csla.Server.Hosts.Silverlight")]
    public partial class UpdateRequest {
        
        private byte[] clientContextField;
        
        private string clientCultureField;
        
        private string clientUICultureField;
        
        private byte[] globalContextField;
        
        private byte[] objectDataField;
        
        private byte[] principalField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", IsNullable=true)]
        public byte[] ClientContext {
            get {
                return this.clientContextField;
            }
            set {
                this.clientContextField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ClientCulture {
            get {
                return this.clientCultureField;
            }
            set {
                this.clientCultureField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ClientUICulture {
            get {
                return this.clientUICultureField;
            }
            set {
                this.clientUICultureField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", IsNullable=true)]
        public byte[] GlobalContext {
            get {
                return this.globalContextField;
            }
            set {
                this.globalContextField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", IsNullable=true)]
        public byte[] ObjectData {
            get {
                return this.objectDataField;
            }
            set {
                this.objectDataField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", IsNullable=true)]
        public byte[] Principal {
            get {
                return this.principalField;
            }
            set {
                this.principalField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/Csla.Server.Hosts.Silverlight")]
    public partial class WcfErrorInfo {
        
        private string exceptionTypeNameField;
        
        private WcfErrorInfo innerErrorField;
        
        private string messageField;
        
        private string sourceField;
        
        private string stackTraceField;
        
        private string targetSiteNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ExceptionTypeName {
            get {
                return this.exceptionTypeNameField;
            }
            set {
                this.exceptionTypeNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public WcfErrorInfo InnerError {
            get {
                return this.innerErrorField;
            }
            set {
                this.innerErrorField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Source {
            get {
                return this.sourceField;
            }
            set {
                this.sourceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string StackTrace {
            get {
                return this.stackTraceField;
            }
            set {
                this.stackTraceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string TargetSiteName {
            get {
                return this.targetSiteNameField;
            }
            set {
                this.targetSiteNameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/Csla.Server.Hosts.Silverlight")]
    public partial class WcfResponse {
        
        private WcfErrorInfo errorDataField;
        
        private byte[] globalContextField;
        
        private byte[] objectDataField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public WcfErrorInfo ErrorData {
            get {
                return this.errorDataField;
            }
            set {
                this.errorDataField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", IsNullable=true)]
        public byte[] GlobalContext {
            get {
                return this.globalContextField;
            }
            set {
                this.globalContextField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", IsNullable=true)]
        public byte[] ObjectData {
            get {
                return this.objectDataField;
            }
            set {
                this.objectDataField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void CreateCompletedEventHandler(object sender, CreateCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CreateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CreateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public WcfResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((WcfResponse)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void FetchCompletedEventHandler(object sender, FetchCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class FetchCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal FetchCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public WcfResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((WcfResponse)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void UpdateCompletedEventHandler(object sender, UpdateCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UpdateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UpdateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public WcfResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((WcfResponse)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void DeleteCompletedEventHandler(object sender, DeleteCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeleteCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal DeleteCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public WcfResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((WcfResponse)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591