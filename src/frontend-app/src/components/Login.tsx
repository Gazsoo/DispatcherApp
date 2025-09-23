import { useState } from "react";
import { useNavigate } from "react-router";
import { AuthService } from "../auth/auth-service"
import type { ReactFormState } from "react-dom/client";

const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  const handleLogin = async (e:React.FormEvent) => {
    e.preventDefault();
    try {
      await AuthService.login(email, password); // Auth service handles everything
      navigate('/dashboard');
    } catch (error) {
      console.error('Login failed:', error);
    }
  };

  return (
    <form onSubmit={handleLogin}>
      <input 
        type= "email"
        value={email} 
        onChange={(e) => setEmail(e.target.value)} 
      />
      <input 
        type="password" 
        value={password} 
        onChange={(e) => setPassword(e.target.value)} 
      />
      <button type="submit">Login</button>
    </form>
  );
};

export default Login;