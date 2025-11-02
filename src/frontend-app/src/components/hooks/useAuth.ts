import { useState, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { AuthService } from '../../auth/auth-service';
import { JWTManager } from '../../auth/jwt/jwt-manager';
import { useUser } from '../context/userContext';

export function useAuth() {
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState('');
    const { setUser, setUserRoles } = useUser();
    const navigate = useNavigate();

    const login = useCallback(async (email: string, password: string) => {
        setError('');
        setIsLoading(true);

        try {
            await AuthService.login(email, password);
            const userInfo = await AuthService.getUserInfo();
            setUser(userInfo);

            setUserRoles?.(JWTManager.getPrimaryRole());
            navigate('/dashboard');
        } catch (error) {
            console.error('Login failed:', error);
            setError('Invalid email or password');
            throw error;
        } finally {
            setIsLoading(false);
        }
    }, [navigate, setUser, setUserRoles]);

    const register = useCallback(async (email: string, password: string, firstName: string, lastName: string, role: string) => {
        setError('');
        setIsLoading(true);
        try {
            await AuthService.register(email, password, firstName, lastName, role);
            return true;
        } catch (err) {
            console.error('Registration failed:', err);
            setError('Registration failed. Please try again.');
            throw err;
        } finally {
            setIsLoading(false);
        }
    }, []);

    const logout = useCallback(() => {
        AuthService.logout();
        setUser(null);
        setUserRoles?.(null);
        navigate('/login');
    }, [navigate, setUser, setUserRoles]);

    return { login, logout, register, isLoading, error };
}
