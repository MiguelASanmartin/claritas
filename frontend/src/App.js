import React, { useState } from 'react';
import './App.css';
import UserList from './components/UserList';
import ProjectList from './components/ProjectList';
import TaskList from './components/TaskList';
import CreateUserForm from './components/CreateUserForm';
import CreateProjectForm from './components/CreateProjectForm';

function App() {
  const [selectedProjectId, setSelectedProjectId] = useState(null);

  const handleProjectClick = (projectId) => {
    console.log("Proyecto clickeado:", projectId);
    setSelectedProjectId(projectId);
  };


  return (
    <div className="App">
      <header className="app-header">
        <h1>Task Management System</h1>
        <p>Clean Architecture + React Frontend</p>
      </header>
      
      <main className="app-main">
        <div className="dashboard">
          <h2>Dashboard</h2>
          <p>Backend API running at: http://localhost:5196</p>
          <p>React frontend running at: http://localhost:3000</p>
        </div>        

        <CreateUserForm />
        <CreateProjectForm />

        <UserList />
        <ProjectList 
          onProjectClick={handleProjectClick} 
          selectedProjectId={selectedProjectId}/>
        <TaskList projectId={selectedProjectId} />
      </main>
    </div>
  );
}

export default App;
