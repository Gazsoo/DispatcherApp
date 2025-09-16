import { useEffect, useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import { AuthenticatedApiClient } from './services/jwt/AuthenticatedApiClient'
import { BrowserRouter } from "react-router";
import { useNavigate } from "react-router";

function App() {
  const navigate = useNavigate();
  const [count, setCount] = useState(0)
  const [user, setUser] = useState(null)
  const a = new AuthenticatedApiClient()

  
  useEffect(() => {
    const handleAuthLogout = (event: CustomEvent) => {
      console.log('Authentication failed:', event.detail);

      // Your app decides what to do:
      // - Show a toast notification
      // - Redirect to login page  
      // - Show a modal asking user to re-authenticate
      // - Clear user state

      navigate('/login');
      // or
      setUser(null);
      // or  
      console.log('Session expired. Please log in again.');
    };

    window.addEventListener('auth:logout', handleAuthLogout as EventListener);

    return () => {
      window.removeEventListener('auth:logout', handleAuthLogout as EventListener);
    };
  }, []);
  
  a.auth_Login({ email: "administrator@localhost", password: "Administrator1!" })

  return (
    <>
      <div>
        <a href="https://vite.dev" target="_blank">
          <img src={viteLogo} className="logo" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" />
        </a>
      </div>
      <h1>Vite + React</h1>
      <div className="card">
        <button onClick={() => setCount((count) => count + 1)}>
          count is {count}
        </button>
        <p>
          Edit <code>src/App.tsx</code> and save to test HMR
        </p>
      </div>
      <p className="read-the-docs">
        Click on the Vite and React logos to learn more
      </p>
    </>
  )
}

export default App
