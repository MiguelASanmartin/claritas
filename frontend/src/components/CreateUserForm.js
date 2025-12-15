import React, {useState} from "react";

function CreateUserForm(){

    const [name, setName] = useState('');
    const [email, setEmail] = useState('');

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();

        setError(null);
        setSuccess(false);

        if (!name.trim()){
            setError('Name is required');
            return;
        }

        if (!email.trim()){
            setError('Email is required');
            return;
        }

        if (!email.includes('@')){
            setError('Please enter a valid email');
            return;
        }

        try{
            setLoading(true);

            const response = await fetch('http://localhost:5196/api/users', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',                    
                },
                body: JSON.stringify({
                    name: name.trim(),
                    email: email.trim()
                })
            });

            if (!response.ok){
                throw new Error(`Error: ${response.status}`);
            }

            setSuccess(true);
            setName('');
            setEmail('');

            setTimeout(() => setSuccess(false), 3000);
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="create-user-form">
            <h3>Create New User</h3>

            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label htmlFor="name">Name:</label>
                    <input
                        id="name"
                        type="text"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        disabled={loading}
                        placeholder="Enter user name"
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="email">Email:</label>
                    <input
                        id="email"
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        disabled={loading}
                        placeholder="user@example.com"
                    />
                </div>

                {error && (
                    <div className="form-error">
                        {error}
                    </div>
                )}

                {success && (
                    <div className="form-success">
                        User created successfully!
                    </div>
                )}

                <button
                    type="submit"
                    disabled={loading}
                    className="submit-btn"
                >
                    {loading ? 'Creating...' : 'Create user'}
                </button>
            </form>
        </div>
    );
}

export default CreateUserForm;