// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 1.1.4322.2032
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

// 
// This source code was auto-generated by Mono Web Services Description Language Utility
//


using System.Collections; //ArrayList

/// <remarks/>
/// <remarks>
///ChronojumpServer
///</remarks>
[System.Web.Services.WebServiceBinding(Name="ChronojumpServerSoap", Namespace="http://localhost:8080/")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public class ChronojumpServer : System.Web.Services.Protocols.SoapHttpClientProtocol {
    
    public ChronojumpServer() {
        this.Url = "http://localhost:8080/chronojumpServer.asmx";
    }
    
    /// <remarks>
///Conecta BBDD
///</remarks>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost:8080/ConnectDatabase", RequestNamespace="http://localhost:8080/", ResponseNamespace="http://localhost:8080/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
    public string ConnectDatabase() {
        object[] results = this.Invoke("ConnectDatabase", new object[0]);
        return ((string)(results[0]));
    }
    
    public System.IAsyncResult BeginConnectDatabase(System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("ConnectDatabase", new object[0], callback, asyncState);
    }
    
    public string EndConnectDatabase(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks>
///DisConecta BBDD
///</remarks>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost:8080/DisConnectDatabase", RequestNamespace="http://localhost:8080/", ResponseNamespace="http://localhost:8080/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
    public string DisConnectDatabase() {
        object[] results = this.Invoke("DisConnectDatabase", new object[0]);
        return ((string)(results[0]));
    }
    
    public System.IAsyncResult BeginDisConnectDatabase(System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("DisConnectDatabase", new object[0], callback, asyncState);
    }
    
    public string EndDisConnectDatabase(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
   
    /// <remarks>
///Stats
///important: variable names here have to be the same than in ChronojumpServerCSharp.cs
///</remarks>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost:8080/Stats", RequestNamespace="http://localhost:8080/", ResponseNamespace="http://localhost:8080/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
    public ArrayList Stats() {
        object[] results = this.Invoke("Stats", new object[0]);
        return ((ArrayList)(results[0]));
    }
    
    public System.IAsyncResult BeginStats(System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("Stats", new object[0], callback, asyncState);
    }
    
    public ArrayList EndStats(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((ArrayList)(results[0]));
    }

    /// <remarks>
///Upload session
///important: variable names here have to be the same than in ChronojumpServerCSharp.cs
///</remarks>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost:8080/UploadSession", RequestNamespace="http://localhost:8080/", ResponseNamespace="http://localhost:8080/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
    public int UploadSession(ServerSession mySession) {
        object[] results = this.Invoke("UploadSession", new object[] {
            mySession});
        return ((int)(results[0]));
    }
    
    public System.IAsyncResult BeginUploadSession(ServerSession mySession, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("UploadSession", new object[] {
            mySession}, callback, asyncState);
    }
    
    public int EndUploadSession(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }

    /// <remarks>
///Update session
///important: variable names here have to be the same than in ChronojumpServerCSharp.cs
///</remarks>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost:8080/UpdateSession", RequestNamespace="http://localhost:8080/", ResponseNamespace="http://localhost:8080/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
    public int UpdateSession(int sessionID, Constants.ServerSessionStates state) {
        object[] results = this.Invoke("UpdateSession", new object[] {
            sessionID, state});
        return ((int)(results[0]));
    }
    
    public System.IAsyncResult BeginUpdateSession(int sessionID, Constants.ServerSessionStates state, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("UpdateSession", new object[] {
            sessionID, state}, callback, asyncState);
    }
    
    public int EndUpdateSession(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }

    /// <remarks>
///Upload an sport (user defined)
///important: variable names here have to be the same than in ChronojumpServerCSharp.cs
///</remarks>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost:8080/UploadSport", RequestNamespace="http://localhost:8080/", ResponseNamespace="http://localhost:8080/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
    public int UploadSport(Sport mySport) {
        object[] results = this.Invoke("UploadSport", new object[] {
            mySport});
        return ((int)(results[0]));
    }
    
    public System.IAsyncResult BeginUploadSport(Sport mySport, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("UploadSport", new object[] {
            mySport}, callback, asyncState);
    }
    
    public int EndUploadSport(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }

    /// <remarks>
///Upload a jump type (user defined)
///important: variable names here have to be the same than in ChronojumpServerCSharp.cs
///</remarks>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost:8080/UploadJumpType", RequestNamespace="http://localhost:8080/", ResponseNamespace="http://localhost:8080/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
    public string UploadJumpType(JumpType type, int evalSID) {
        object[] results = this.Invoke("UploadJumpType", new object[] {
            type, evalSID});
        return ((string)(results[0]));
    }
    
    public System.IAsyncResult BeginUploadJumpType(JumpType type, int evalSID, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("UploadJumpType", new object[] {
            type, evalSID}, callback, asyncState);
    }
    
    public string EndUploadJumpType(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }

    /// <remarks>
///Upload person
///important: variable names here have to be the same than in ChronojumpServerCSharp.cs
///</remarks>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost:8080/UploadPerson", RequestNamespace="http://localhost:8080/", ResponseNamespace="http://localhost:8080/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
    public int UploadPerson(Person myPerson, int sessionID) {
        object[] results = this.Invoke("UploadPerson", new object[] {
            myPerson, sessionID});
        return ((int)(results[0]));
    }
    
    public System.IAsyncResult BeginUploadPerson(Person myPerson, int sessionID, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("UploadPerson", new object[] {
            myPerson, sessionID}, callback, asyncState);
    }
    
    public int EndUploadPerson(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }

    /// <remarks>
///Upload person session if needed
///important: variable names here have to be the same than in ChronojumpServerCSharp.cs
///</remarks>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost:8080/UploadPersonSessionIfNeeded", RequestNamespace="http://localhost:8080/", ResponseNamespace="http://localhost:8080/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
    public int UploadPersonSessionIfNeeded(int personServerID, int sessionServerID, int weight) {
        object[] results = this.Invoke("UploadPersonSessionIfNeeded", new object[] {
            personServerID, sessionServerID, weight});
        return ((int)(results[0]));
    }
    
    public System.IAsyncResult BeginUploadPersonSessionIfNeeded(int personServerID, int sessionServerID, int weight, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("UploadPersonSessionIfNeeded", new object[] {
            personServerID, sessionServerID, weight}, callback, asyncState);
    }
    
    public int EndUploadPersonSessionIfNeeded(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }


    /// <remarks>
///Upload ping
///important: variable names here have to be the same than in ChronojumpServerCSharp.cs
///</remarks>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost:8080/UploadPing", RequestNamespace="http://localhost:8080/", ResponseNamespace="http://localhost:8080/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
    public int UploadPing(ServerPing myPing, bool doInsertion) {
        object[] results = this.Invoke("UploadPing", new object[] {
            myPing, doInsertion});
        return ((int)(results[0]));
    }
    
    public System.IAsyncResult BeginUploadPing(ServerPing myPing, bool doInsertion, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("UploadPing", new object[] {
            myPing, doInsertion}, callback, asyncState);
    }
    
    public int EndUploadPing(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }

    /// <remarks>
///Upload evaluator
///important: variable names here have to be the same than in ChronojumpServerCSharp.cs
///</remarks>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost:8080/UploadEvaluator", RequestNamespace="http://localhost:8080/", ResponseNamespace="http://localhost:8080/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
    public int UploadEvaluator(ServerEvaluator myEval) {
        object[] results = this.Invoke("UploadEvaluator", new object[] {
            myEval});
        return ((int)(results[0]));
    }
    
    public System.IAsyncResult BeginUploadEvaluator(ServerEvaluator myEval, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("UploadEvaluator", new object[] {
            myEval}, callback, asyncState);
    }
    
    public int EndUploadEvaluator(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }


    /// <remarks>
///Upload a jump
///important: variable names here have to be the same than in ChronojumpServerCSharp.cs
///also don't pass a Event expecting that server can use polymorphism like if it's jump
///</remarks>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost:8080/UploadJump", RequestNamespace="http://localhost:8080/", ResponseNamespace="http://localhost:8080/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
    public int UploadJump(Jump myTest) {
        object[] results = this.Invoke("UploadJump", new object[] {
            myTest});
        return ((int)(results[0]));
    }
    
    public System.IAsyncResult BeginUploadJump(Jump myTest, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("UploadJump", new object[] {
            myTest}, callback, asyncState);
    }
    
    public int EndUploadJump(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }

    /// <remarks>
///Upload a jumpRj
///important: variable names here have to be the same than in ChronojumpServerCSharp.cs
///also don't pass a Event expecting that server can use polymorphism like if it's jump
///</remarks>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost:8080/UploadJumpRj", RequestNamespace="http://localhost:8080/", ResponseNamespace="http://localhost:8080/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
    public int UploadJumpRj(JumpRj myTest) {
        object[] results = this.Invoke("UploadJumpRj", new object[] {
            myTest});
        return ((int)(results[0]));
    }
    
    public System.IAsyncResult BeginUploadJumpRj(JumpRj myTest, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("UploadJumpRj", new object[] {
            myTest}, callback, asyncState);
    }
    
    public int EndUploadJumpRj(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }

    /// <remarks>
///Upload a run
///important: variable names here have to be the same than in ChronojumpServerCSharp.cs
///also don't pass a Event expecting that server can use polymorphism like if it's jump
///</remarks>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost:8080/UploadRun", RequestNamespace="http://localhost:8080/", ResponseNamespace="http://localhost:8080/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
    public int UploadRun(Run myTest) {
        object[] results = this.Invoke("UploadRun", new object[] {
            myTest});
        return ((int)(results[0]));
    }
    
    public System.IAsyncResult BeginUploadRun(Run myTest, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("UploadRun", new object[] {
            myTest}, callback, asyncState);
    }
    
    public int EndUploadRun(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }

    /// <remarks>
///Upload a run interval
///important: variable names here have to be the same than in ChronojumpServerCSharp.cs
///also don't pass a Event expecting that server can use polymorphism like if it's jump
///</remarks>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost:8080/UploadRunI", RequestNamespace="http://localhost:8080/", ResponseNamespace="http://localhost:8080/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
    public int UploadRunI(RunInterval myTest) {
        object[] results = this.Invoke("UploadRunI", new object[] {
            myTest});
        return ((int)(results[0]));
    }
    
    public System.IAsyncResult BeginUploadRunI(RunInterval myTest, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("UploadRunI", new object[] {
            myTest}, callback, asyncState);
    }
    
    public int EndUploadRunI(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }

    /// <remarks>
///Upload a reaction time
///important: variable names here have to be the same than in ChronojumpServerCSharp.cs
///also don't pass a Event expecting that server can use polymorphism like if it's jump
///</remarks>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost:8080/UploadRT", RequestNamespace="http://localhost:8080/", ResponseNamespace="http://localhost:8080/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
    public int UploadRT(ReactionTime myTest) {
        object[] results = this.Invoke("UploadRT", new object[] {
            myTest});
        return ((int)(results[0]));
    }
    
    public System.IAsyncResult BeginUploadRT(ReactionTime myTest, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("UploadRT", new object[] {
            myTest}, callback, asyncState);
    }
    
    public int EndUploadRT(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }

    /// <remarks>
///Upload a pulse
///important: variable names here have to be the same than in ChronojumpServerCSharp.cs
///also don't pass a Event expecting that server can use polymorphism like if it's jump
///</remarks>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost:8080/UploadPulse", RequestNamespace="http://localhost:8080/", ResponseNamespace="http://localhost:8080/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
    public int UploadPulse(Pulse myTest) {
        object[] results = this.Invoke("UploadPulse", new object[] {
            myTest});
        return ((int)(results[0]));
    }
    
    public System.IAsyncResult BeginUploadPulse(Pulse myTest, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("UploadPulse", new object[] {
            myTest}, callback, asyncState);
    }
    
    public int EndUploadPulse(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }

    /// <remarks>
///List directory files (only as a sample)
///important: variable names here have to be the same than in ChronojumpServerCSharp.cs
///</remarks>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost:8080/ListDirectory", RequestNamespace="http://localhost:8080/", ResponseNamespace="http://localhost:8080/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
    public string[] ListDirectory(string path) {
        object[] results = this.Invoke("ListDirectory", new object[] {
            path});
        return ((string[])(results[0]));
    }
    
    public System.IAsyncResult BeginListDirectory(string path, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("ListDirectory", new object[] {
            path}, callback, asyncState);
    }
    
    public string[] EndListDirectory(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string[])(results[0]));
    }
   
    /* 
    /// <remarks>
///Select person name
///important: variable names here have to be the same than in ChronojumpServerCSharp.cs
///</remarks>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost:8080/SelectPersonName", RequestNamespace="http://localhost:8080/", ResponseNamespace="http://localhost:8080/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
    public string SelectPersonName(int personID) {
        object[] results = this.Invoke("SelectPersonName", new object[] {
            personID});
        return ((string)(results[0]));
    }
    
    public System.IAsyncResult BeginSelectPersonName(int personID, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("SelectPersonName", new object[] {
            personID}, callback, asyncState);
    }
    
    public string EndSelectPersonName(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks>
///See all persons
///important: variable names here have to be the same than in ChronojumpServerCSharp.cs
///</remarks>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost:8080/SelectAllPersons", RequestNamespace="http://localhost:8080/", ResponseNamespace="http://localhost:8080/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
    public ArrayList SelectAllPersons() {
        object[] results = this.Invoke("SelectAllPersons", new object[0]);
        return ((ArrayList)(results[0]));
    }
    
    public System.IAsyncResult BeginSelectAllPersons(System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("SelectAllPersons", new object[0], callback, asyncState);
    }
    
//public string[] EndSelectAllPersons(System.IAsyncResult asyncResult) {
    public ArrayList EndSelectAllPersons(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
//return ((string[])(results[0]));
        return ((ArrayList)(results[0]));
    }
    */

/*    
    /// <remarks>
///Select events from all persons
///important: variable names here have to be the same than in ChronojumpServerCSharp.cs
///</remarks>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost:8080/SelectAllPersonEvents", RequestNamespace="http://localhost:8080/", ResponseNamespace="http://localhost:8080/", ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped, Use=System.Web.Services.Description.SoapBindingUse.Literal)]
    public object[] SelectAllPersonEvents(int personID) {
        object[] results = this.Invoke("SelectAllPersonEvents", new object[] {
            personID});
        return ((object[])(results[0]));
    }
    
    public System.IAsyncResult BeginSelectAllPersonEvents(int personID, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("SelectAllPersonEvents", new object[] {
            personID}, callback, asyncState);
    }
    
    public object[] EndSelectAllPersonEvents(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((object[])(results[0]));
    }
  */

}
