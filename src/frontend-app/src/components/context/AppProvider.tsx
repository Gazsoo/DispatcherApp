import { type ReactNode } from 'react';
import { UserProvider } from './userContext';

/**
 * Centralized provider composition
 */
export const AppProviders = ({ children }: { children: ReactNode }) => {
    return (
        <UserProvider>
            {children}
        </UserProvider>
    );
};