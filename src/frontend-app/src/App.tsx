import { useEffect, useState } from 'react'
import reactLogo from './assets/react.svg'
import goggle from '/src/assets/goggle.png'
import goggleLight from '/src/assets/gogglewhite.png'
import { AuthenticatedApiClient } from './services/authenticated-api-client'
import { BrowserRouter, Link } from "react-router";
import { useNavigate } from "react-router";
import ThemeToggle from './components/ThemeToggle';

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
  

return (
    <div className="min-h-screen bg-surface-light dark:bg-surface-dark text-content-light dark:text-content-dark transition-colors duration-200">
      
      <div className="fixed top-4 right-4 z-50">
        <ThemeToggle />
      </div>

      <div className="flex flex-col items-center justify-center min-h-screen p-2">
              <img 
                src={goggleLight} 
                alt="Logo" 
                className="block dark:hidden h-24 w-60 drop-shadow-accent mb-10"
              />
              
              {/* Dark mode image */}
              <img 
                src={goggle}
                alt="Logo" 
                className="hidden dark:block h-24 w-60 drop-shadow-accent mb-10"
              />
            {/* <img 
              src={goggle} 
              className="h-24 w-60 drop-shadow-accent mb-10" 
              alt="Goggle logo" 
            /> */}

        {/* Title */}
        <h1 className="text-5xl font-bold mb-3 tracking-tight bg-gradient-to-r from-accent to-accent-dark bg-clip-text text-transparent">
          Dispatcher App
        </h1>

        <div className="bg-surface-light-card dark:bg-surface-dark-card backdrop-blur-sm rounded-2xl p-4 shadow-xl border border-surface-light-border dark:border-surface-dark-border max-w-sm w-full">
          <Link 
            to="/login" 
            className="block w-full bg-gradient-to-r from-accent to-accent-dark hover:from-accent-dark hover:to-accent text-white font-semibold py-3 px-6 rounded-lg mb-4 text-center transition-all duration-300 shadow-lg hover:shadow-xl"
          >
            Login
          </Link>
          <button 
            className="w-full bg-surface-light-border dark:bg-surface-dark-border hover:bg-accent/20 dark:hover:bg-accent/20 text-content-light dark:text-content-dark font-medium py-3 px-6 rounded-lg transition-all duration-200 border-2 border-transparent hover:border-accent"
          >
            Help
          </button>
          
        </div>

      </div>
    </div>
)
}

export default App