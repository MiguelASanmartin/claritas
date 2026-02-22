import React, {useState, useEffect } from 'react';

function TaskList({ projectId }) {
    // Estado para almacenar tareas
    const [tasks, setTasks] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    // Hook para obetner datos al cargar el componente
    useEffect(() => {
        fetchTasks();
    }, [projectId]); // [projectId] significa "ejecutar solo una al montar y cuando cambie projectId"

    // Función para obtener tareas de tu API
    const fetchTasks = async () => {
        try{
            setLoading(true);
            
            if (!projectId){
                return;
            }

            const response = await fetch(`http://localhost:5196/api/tasks/by-project/${projectId}`);

            if (!response.ok){
                throw new Error(`Error: ${response.status}`);
            }

            const data = await response.json();
            setTasks(data);
            setError(null);            
        } catch (err){
            setError(err.message);
            setTasks([]);            
        } finally {
            setLoading(false);
        }
    };

    if (loading){
        return <div className='loading'>Loading tasks...</div>;
    }

    if (error){
        return (
            <div className='error'>
                <p>Error loading tasks: {error}</p>
                <button onClick={fetchTasks}>Retry</button>
            </div>
        );
    }

    if (!projectId){
        return (
            <div className="task-list">
                <div className="section-header">
                    <h3>Tasks ({tasks.length})</h3>
                </div>
                <p className="no-data">Select a project to view its tasks</p>
            </div>
        )
    }

    return (
        <div className="task-list">
            <div className="section-header">
                <h3>
                    Tasks ({tasks.length})
                    {projectId && <span> - Filtered by project</span>}
                </h3>
                <button onClick={fetchTasks} className="refresh-btn">
                    Refresh
                </button>
            </div>

            {tasks.length === 0 ? (
                <p className="no-data">No tasks found. Create some tasks first!</p>
            ) : (
                <div className="task-grid">
                    {tasks.map(task => (
                        <div key={task.id} className="task-card">
                            <div className="task-header">
                                <h4>{task.title}</h4>
                                <span className={`priority-badge priority-${task.priority.toLowerCase()}`}>
                                    {task.priority}
                                </span>
                            </div>                
                            <span className={`status-badge status-${task.status.toLowerCase()}`}>
                                {task.status}
                            </span>
                            <p>{task.description}</p>
                            <p>{task.projectName}</p>
                            <p>{task.assignedToUserName}</p>
                            <small>ID: {task.id}</small>
                        </div>
                    ))}
                </div>
            )}     
        </div>
    );
}

export default TaskList;