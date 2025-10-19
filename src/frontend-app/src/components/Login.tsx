import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import ThemeToggle from './ThemeToggle';
import { Input, Button, Card } from './ui';
import { useAuth } from "./hooks/useAuth";
import { useUser } from "./context/userContext";

const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const { login, isLoading, error } = useAuth();
  const { user, isLoading: isCheckingAuth } = useUser();
  const navigate = useNavigate();

  // Redirect if already logged in
  useEffect(() => {
    if (!isCheckingAuth && user) {
      navigate('/dashboard');
    }
  }, [user, isCheckingAuth, navigate]);

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    await login(email, password);
  };

  return (
    <div className="min-h-screen bg-surface-light dark:bg-surface-dark text-content-light dark:text-content-dark transition-colors duration-200">

      <div className="fixed top-4 right-4 z-50">
        <ThemeToggle />
      </div>

      <div className="flex flex-col items-center justify-center min-h-screen p-2">

        <h1 className="text-5xl font-bold mb-3 pb-3 tracking-tight bg-gradient-to-r from-accent to-accent-dark bg-clip-text text-transparent">
          Login
        </h1>

        <Card>
          <form onSubmit={handleLogin} className="space-y-6">
            <Input
              id="email"
              label="Email"
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              placeholder="your@email.com"
            />

            <Input
              id="password"
              label="Password"
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              placeholder="••••••••"
            />

            {error && (
              <div className="text-red-500 text-sm text-center">
                {error}
              </div>
            )}

            <Button type="submit" variant="primary" isLoading={isLoading}>
              Sign In
            </Button>
          </form>

          <Button
            onClick={() => navigate('/')}
            variant="secondary"
            className="mt-4"
          >
            Back to Home
          </Button>
        </Card>
      </div>
    </div>
  );
};

export default Login;
