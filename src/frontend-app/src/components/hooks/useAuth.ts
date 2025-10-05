import { useEffect, useState, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { AuthService } from '../../auth/auth-service';
import { useUser } from '../context/userContext';

export function useAuth() {
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState('');
    const { setUser } = useUser();
    const navigate = useNavigate();

    const login = useCallback(async (email: string, password: string) => {
        setError('');
        setIsLoading(true);

        try {
            await AuthService.login(email, password);
            const userInfo = await AuthService.getUserInfo();
            setUser(userInfo);
            navigate('/dashboard');
        } catch (error) {
            console.error('Login failed:', error);
            setError('Invalid email or password');
            throw error;
        } finally {
            setIsLoading(false);
        }
    }, [navigate, setUser]);

    const logout = useCallback(() => {
        AuthService.logout();
        setUser(null);
        navigate('/login');
    }, [navigate, setUser]);

    return { login, logout, isLoading, error };
}