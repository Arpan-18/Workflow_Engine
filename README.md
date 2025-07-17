# Workflow_Engine
Infonetica - Workflow Engine API
A lightweight backend service built using .NET 8 and C# Minimal API that allows you to define and execute custom state-machine-based workflows with simple in-memory storage.

🔧 Features
Define workflows with customizable states and transitions

Start and manage multiple workflow instances

Validate and execute transitions

Simple in-memory persistence (no database)

Minimal, clean REST API

🚀 Getting Started
1. Clone the Repository
bash
Copy
Edit
git clone https://github.com/Arpan-18/workflowengine.git
cd workflowengine
2. Run the Application
bash
Copy
Edit
dotnet run
3. Test the API (using Postman, curl, etc.)
Example: Create a Workflow
h
Copy
Edit
POST /workflows
Content-Type: application/json

{
  "Id": "sample-wf",
  "States": [
    { "Id": "start", "IsInitial": true, "IsFinal": false, "Enabled": true },
    { "Id": "process", "IsInitial": false, "IsFinal": false, "Enabled": true },
    { "Id": "complete", "IsInitial": false, "IsFinal": true, "Enabled": true }
  ],
  "Actions": [
    { "Id": "next", "Enabled": true, "FromStates": ["start"], "ToState": "process" },
    { "Id": "done", "Enabled": true, "FromStates": ["process"], "ToState": "complete" }
  ]
}
📡 API Endpoints
Method	Endpoint	Description
POST	/workflows	Create a new workflow definition
GET	/workflows	Get all workflow definitions
GET	/workflows/{id}	Get a specific workflow definition
POST	/instances/{workflowId}	Start a new workflow instance
POST	/instances/{instanceId}/actions/{actionId}	Perform an action on an instance
GET	/instances	Get all running workflow instances
GET	/instances/{id}	Get details of a specific instance

📌 Notes
Only in-memory storage is used — data is not persisted after shutdown.

No authentication or multi-user handling is implemented.

Workflow IDs must be unique.

State and Action IDs can be reused across workflows.

🧩 Future Scope
File-based or database-backed persistence (JSON/YAML/DB)

Unit tests using xUnit or NUnit

Swagger/OpenAPI integration

Role or permission-based access (RBAC)

🛠 Tech Stack
.NET 8

ASP.NET Core Minimal API

C# 12

No third-party packages

📬 Contact
For clarifications or improvements, feel free to reach out.

Developed as part of the Infonetica Software Engineer Intern Assignment.
