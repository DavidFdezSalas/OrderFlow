/* eslint-disable @typescript-eslint/no-unused-vars */
// Login.tsx
import React, { useState, type FormEvent } from 'react';
// import './Login.css'; // Asegúrate de tener este archivo CSS

interface UserData {
    user: string;
    token: string;
}

interface LoginProps {
    onLoginSuccess: (data: UserData) => void;
}

const Login: React.FC<LoginProps> = ({ onLoginSuccess }) => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const handleSubmit = async (e: FormEvent) => {
        e.preventDefault();
        setError('');
        setLoading(true);

        if (!email || !password) {
            setError('Por favor, ingresa tu email y contraseña.');
            setLoading(false);
            return;
        }

        // --- Simulación de Lógica de Autenticación para testear la integración ---
        try {
            await new Promise(resolve => setTimeout(resolve, 500)); // Simula un retraso de red

            // **Usuario de prueba para verificar la integración:**
            if (email === 'test@shop.com' && password === 'password123') {
                // Llama a la prop para actualizar el estado en App.tsx
                onLoginSuccess({
                    user: email,
                    token: 'fake-jwt-token-123'
                });
            } else {
                setError('Email o contraseña incorrectos. Usa test@shop.com / password123');
            }
        } catch (apiError) {
            setError('Ocurrió un error de conexión.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="login-container">
            <h2>Iniciar Sesión en Shop</h2>
            <form onSubmit={handleSubmit} className="login-form">
                {error && <p className="error-message">{error}</p>}

                <div className="form-group">
                    <label htmlFor="email">Email</label>
                    <input
                        type="email"
                        id="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                        disabled={loading}
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="password">Contraseña</label>
                    <input
                        type="password"
                        id="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                        disabled={loading}
                    />
                </div>

                <button type="submit" disabled={loading} className="login-button">
                    {loading ? 'Verificando...' : 'Entrar'}
                </button>
            </form>
        </div>
    );
};

export default Login;