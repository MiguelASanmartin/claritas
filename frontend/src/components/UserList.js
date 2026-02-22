import React, { useState, useEffect } from 'react';

function UserList() {
  // Estado para almacenar usuarios
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Hook para obtener datos al cargar componente
  useEffect(() => {
    fetchUsers();
  }, []); // [] significa "ejecutar solo una vez al montar"

  // Función para obtener usuarios de tu API
  const fetchUsers = async () => {
    try {
      setLoading(true);
      const response = await fetch('http://localhost:5196/api/users');
      
      if (!response.ok) {
        throw new Error(`Error: ${response.status}`);
      }
      
      const data = await response.json();
      setUsers(data);
      setError(null);
    } catch (err) {
      setError(err.message);
      setUsers([]);
    } finally {
      setLoading(false);
    }
  };

  // Renderizado condicional
  if (loading) {
    return <div className="loading">Loading users...</div>;
  }

  if (error) {
    return (
      <div className="error">
        <p>Error loading users: {error}</p>
        <button onClick={fetchUsers}>Retry</button>
      </div>
    );
  }

  return (
    <div className="user-list">
      <div className="section-header">
        <h3>Users ({users.length})</h3>
        <button onClick={fetchUsers} className="refresh-btn">
          Refresh
        </button>
      </div>

      {users.length === 0 ? (
        <p className="no-data">No users found. Create some users first!</p>
      ) : (
        <div className="user-grid">
          {users.map(user => (
            <div key={user.id} className="user-card">
              <h4>{user.name}</h4>
              <p>{user.email}</p>
              <small>ID: {user.id}</small>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default UserList;