import goggle from './assets/goggle.png?url'
import goggleLight from './assets/gogglewhite.png?url'
import { useNavigate } from "react-router-dom";
import ThemeToggle from './components/ThemeToggle';
import { Button, Card } from './components/ui';
import { useUser } from './components/context/userContext';

function App() {
  const { user } = useUser();
  const navigate = useNavigate();

  const handleLoginClick = () => {
    if (user) {
      navigate('/dashboard');
    } else {
      navigate('/login');
    }
  };
  const handleRegisterClick = () => {
    navigate('/register');
  };

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

        <img
          src={goggle}
          alt="Logo"
          className="hidden dark:block h-32 w-80 drop-shadow-accent mb-10"
        />

        <h1 className="text-5xl font-bold mb-3 tracking-tight bg-gradient-to-r from-accent to-accent-dark bg-clip-text text-transparent">
          Dispatcher App
        </h1>

        <Card className="max-w-sm p-4">
          <Button variant="primary" onClick={handleLoginClick} className="mb-4">
            Login
          </Button>
          <Button variant="secondary" onClick={handleRegisterClick}>Register</Button>
        </Card>

      </div>
    </div>
  )
}

export default App