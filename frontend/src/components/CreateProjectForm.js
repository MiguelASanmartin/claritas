import React, { useState, useEffect } from 'react';

function CreateProjectForm(){
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [ownerId, setOwnerId] = useState('');
    const [dueDate, setDueDate] = useState('');

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(false);

    const [users, setUsers] = useState([]);
    const [loadingUsers, setLoadingUsers] = useState(false);

    useEffect(() => {
        fetchUsers();
    }, []);

    const fetchUsers = async () => {
        try{
            setLoadingUsers(true);

            const response = await fetch('http://localhost:5196/api/users');

            if (!response.ok){
                throw new Error(`Error: ${response.status}`);
            }

            const data = await response.json();
            setUsers(data);
            setError(null);
        } catch (err){
            setError(err.message);
            setUsers([]);            
        } finally {
            setLoadingUsers(false);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        setError(null);
        setSuccess(false);

        if (!name.trim()){
            setError('Name is required');
            return;
        }

        if (!ownerId.trim()){
            setError('Owner is required');
            return;
        }

        try{
            setLoading(true);

            const response = await fetch('http://localhost:5196/api/projects', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    name: name.trim(),
                    description: description,
                    ownerId: ownerId,
                    dueDate: dueDate || null
                })
            });

            if (!response.ok){
                throw new Error(`Error: ${response.status}`);
            }

            setSuccess(true);
            setName('');
            setDescription('');
            setOwnerId('');
            setDueDate('');

            setTimeout(() => setSuccess(false), 3000);
        } catch (err) {
            setError(err.message);
        } finally{
            setLoading(false);
        }
    };

    return (
        <div className="create-project-form">
            <h3>Create New Project</h3>

            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label>Name:</label>
                    <input 
                        id = "name"
                        type="text"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        disabled={loading}
                        placeholder="Enter project name"
                    />
                </div>

                <div className="form-group">
                    <label>Description:</label>
                    <textarea 
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
                        rows="4"
                        placeholder="Optional project description"
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="owner">Owner:</label>
                    {loadingUsers ? (
                        <p className="loading">Loading users...</p>
                    ) : (
                        <select
                            id="owner"
                            value={ownerId}
                            onChange={(e) => setOwnerId(e.target.value)}
                            disabled={loading}
                            >
                                <option value="">-- Select Owner --</option>
                                {users.map(user => (
                                    <option key={user.id} value={user.id}>
                                        {user.name}
                                    </option>
                                ))}
                        </select>
                    )}
                </div>

                <div className="form-group">
                    <label htmlFor="dueDate">Due Date (optional):</label>
                    <input 
                        id="dueDate"
                        type="date"
                        value={dueDate}
                        onChange={(e) => setDueDate(e.target.value)}
                        disabled={loading}
                    />
                </div>

                {error && (
                    <div className="form-error">
                        {error}
                    </div>
                )}

                {success && (
                    <div className="form-success">
                        Project created successfully!
                    </div>
                )}

                <button
                    type="submit"
                    disabled={loading || loadingUsers}
                    className="submit-btn"
                    >
                        {loading ? "Creating..." : "Create Project"}
                    </button>
                
            </form>
        </div>
    );
}

export default CreateProjectForm