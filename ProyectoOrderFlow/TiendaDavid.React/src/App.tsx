import { useState } from 'react';
import './App.css';
import Login from './componentes/Login'; // 👈 Importa tu componente Login

// Tipado básico para los datos del usuario (manteniendo TypeScript)
interface UserData {
    user: string;
    token: string;
}

function App() {
    // 1. Estados para manejar la autenticación
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [userData, setUserData] = useState<UserData | null>(null);

    // 2. Función que se llama al iniciar sesión (recibe datos tipados)
    const handleLoginSuccess = (data: UserData) => {
        setIsLoggedIn(true);
        setUserData(data);
        console.log("¡Usuario autenticado con éxito!");
    };

    // 3. Función de cierre de sesión
    const handleLogout = () => {
        setIsLoggedIn(false);
        setUserData(null);
        console.log("Sesión cerrada.");
    };

    return (
        <>
            {/* Puedes mantener tu estilo general o eliminar esta línea */}
            <div className="app-container">
                <header>
                    <h1>Shop App</h1>
                </header>

                {isLoggedIn ? (
                    // Contenido si el usuario está autenticado
                    <div className="dashboard-view">
                        <h2>Bienvenido a la Tienda, {userData?.user}!</h2>
                        <p>¡Aquí va el catálogo y el resto de tu aplicación!</p>
                        <button onClick={handleLogout} className="logout-button">
                            Cerrar Sesión
                        </button>
                    </div>
                ) : (
                    // Contenido si el usuario NO está autenticado
                    <Login onLoginSuccess={handleLoginSuccess} />
                )}
            </div>
        </>
    );
}

export default App;