import React, { useState, useEffect } from 'react';

function ProjectList({ onProjectClick, selectedProjectId }) {
  // Estado para almacenar proyectos
  const [projects, setProjects] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Hook para obtener datos al cargar componente
  useEffect(() => {
    fetchProjects();
  }, []); // [] significa "ejecutar solo una vez al montar"

  // Función para obtener proyectos de tu API
  const fetchProjects = async () => {
    try {
      setLoading(true);
      const response = await fetch('http://localhost:5196/api/projects');
      
      if (!response.ok) {
        throw new Error(`Error: ${response.status}`);
      }
      
      const data = await response.json();
      setProjects(data);
      setError(null);
    } catch (err) {
      setError(err.message);
      setProjects([]);
    } finally {
      setLoading(false);
    }
  };

  // Renderizado condicional
  if (loading) {
    return <div className="loading">Loading projects...</div>;
  }

  if (error) {
    return (
      <div className="error">
        <p>Error loading projects: {error}</p>
        <button onClick={fetchProjects}>Retry</button>
      </div>
    );
  }

  return (
    <div className="project-list">
      <div className="section-header">
        <h3>Projects ({projects.length})</h3>
        <button onClick={fetchProjects} className="refresh-btn">
          Refresh
        </button>
      </div>

      {projects.length === 0 ? (
        <p className="no-data">No projects found. Create some projects first!</p>
      ) : (
        <div className="project-grid">
          {projects.map(project => (
            <div 
            key={project.id} 
            className={`project-card ${project.id === selectedProjectId ? 'selected' : ''}`}
            onClick={() => onProjectClick(project.id)}
            style={{ cursor: "pointer" }}>
              <h4>{project.name}</h4>
              <p>{project.description}</p>
              <p>{project.ownerName}</p>
              <small>ID: {project.id}</small>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default ProjectList;