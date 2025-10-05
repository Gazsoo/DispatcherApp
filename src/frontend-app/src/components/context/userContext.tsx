import { createContext, useContext, useState, useEffect, type ReactNode, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import { AuthService } from '../../auth/auth-service';
import type { UserInfoResponse } from '../../services/web-api-client';

interface UserContextType {
    user: UserInfoResponse | null;
    setUser: (user: UserInfoResponse | null) => void;
    isLoading: boolean;
}

const UserContext = createContext<UserContextType | null>(null);

export const UserProvider = ({ children }: { children: ReactNode }) => {

    const [user, setUser] = useState<UserInfoResponse | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const navigate = useNavigate();

    // Try to restore session on mount
    useEffect(() => {
        const restoreSession = async () => {
            const userInfo = await AuthService.tryRestoreSession();
            if (userInfo) {
                setUser(userInfo);
            }
            setIsLoading(false);
        };

        restoreSession();
    }, []);

    useEffect(() => {
        const handleAuthLogout = (event: CustomEvent) => {
            console.log('Authentication failed:', event.detail);
            setUser(null);
            navigate('/login');
        };

        window.addEventListener('auth:logout', handleAuthLogout as EventListener);

        return () => {
            window.removeEventListener('auth:logout', handleAuthLogout as EventListener);
        };
    }, [navigate]);
    const value = useMemo(() => ({ user, setUser, isLoading }), [user, isLoading]);

    return (
        <UserContext value={value}>
            {children}
        </UserContext>
    );
};

export const useUser = () => {
    const context = useContext(UserContext);
    if (!context) {
        throw new Error('useUser must be used within a UserProvider');
    }
    return context;
};

export default UserContext;