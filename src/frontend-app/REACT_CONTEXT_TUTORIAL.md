# Comprehensive Guide: React Context Pattern with NSwag Integration

## Part 1: Understanding Your UserContext Pattern

### The Three-Part Architecture

Your UserContext implementation follows a powerful React pattern that consists of **three interconnected pieces**:

#### 1. The Context (`createContext`)
```tsx
const UserContext = createContext<UserContextType | null>(null);
```

**What it does**: Creates a "data channel" that can pass information down through your component tree without manually passing props at every level.

**Why `null` initially**: TypeScript requires an initial value, but we use `null` to force consumers to use the Provider. This prevents accidentally using the context outside the Provider.

#### 2. The Provider (`UserProvider`)
```tsx
export const UserProvider = ({ children }: { children: ReactNode }) => {
    const [user, setUser] = useState<UserInfoResponse | null>(null);
    const navigate = useNavigate();

    useEffect(() => {
        // Event listener setup
    }, [navigate]);

    return (
        <UserContext.Provider value={{ user, setUser }}>
            {children}
        </UserContext.Provider>
    );
};
```

**What it does**:
- Holds the actual state (`user`)
- Manages side effects (auth logout listener)
- Provides the state and updater functions to all child components

**Why this architecture**: By centralizing user state here, you ensure:
- **Single source of truth**: User data lives in one place
- **Automatic re-renders**: Any component using `useUser()` re-renders when user changes
- **Separation of concerns**: Authentication logic is isolated from UI components

#### 3. The Hook (`useUser`)
```tsx
export const useUser = () => {
    const context = useContext(UserContext);
    if (!context) {
        throw new Error('useUser must be used within a UserProvider');
    }
    return context;
};
```

**What it does**: Provides a type-safe, convenient way to access the context with built-in error handling.

**Why the error check**: If someone tries to use `useUser()` outside of `<UserProvider>`, they get a clear error message instead of mysterious `null` errors.

### Key Architectural Benefits

1. **Prop Drilling Elimination**: No need to pass `user` through 5+ component levels
2. **Global State Management**: User state is accessible anywhere in your component tree
3. **Type Safety**: TypeScript ensures you can't access user data incorrectly
4. **Lifecycle Management**: The Provider handles setup/cleanup of event listeners
5. **Testability**: You can easily mock the context in tests

---

## Part 2: Using UserContext with NSwag API Clients

Now let's see how to properly integrate your UserContext with NSwag-generated API clients.

### Pattern 1: Setting User After Login

First, let's update your Login component to use the UserContext:

**File**: `src/components/Login.tsx`

```tsx
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { AuthService } from "../auth/auth-service";
import { useUser } from "./context/userContext"; // Import the hook
import ThemeToggle from './ThemeToggle';
import goggle from '/src/assets/goggle.png';
import goggleLight from '/src/assets/gogglewhite.png';
import { Input, Button, Card } from './ui';

const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();
  const { setUser } = useUser(); // Use the context hook

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setIsLoading(true);

    try {
      // Step 1: Authenticate
      await AuthService.login(email, password);

      // Step 2: Fetch user info and store in context
      const userInfo = await AuthService.getUserInfo();
      setUser(userInfo);

      // Step 3: Navigate to dashboard
      navigate('/dashboard');
    } catch (error) {
      console.error('Login failed:', error);
      setError('Invalid email or password');
    } finally {
      setIsLoading(false);
    }
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
          className="hidden dark:block h-24 w-60 drop-shadow-accent mb-10"
        />

        <h1 className="text-5xl font-bold mb-3 tracking-tight bg-gradient-to-r from-accent to-accent-dark bg-clip-text text-transparent">
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
              disabled={isLoading}
            />

            <Input
              id="password"
              label="Password"
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              placeholder="••••••••"
              disabled={isLoading}
            />

            {error && (
              <div className="rounded-md bg-red-50 dark:bg-red-900/20 p-3 border border-red-200 dark:border-red-800">
                <p className="text-sm text-red-800 dark:text-red-200">
                  {error}
                </p>
              </div>
            )}

            <Button
              type="submit"
              variant="primary"
              disabled={isLoading}
            >
              {isLoading ? 'Signing in...' : 'Sign In'}
            </Button>
          </form>

          <Button
            onClick={() => navigate('/')}
            variant="secondary"
            className="mt-4"
            disabled={isLoading}
          >
            Back to Home
          </Button>
        </Card>
      </div>
    </div>
  );
};

export default Login;
```

**Key improvements**:
1. **Loading state**: Prevents duplicate submissions and provides user feedback
2. **Two-step authentication**: Login, then fetch user info
3. **Context integration**: Stores user info in context for app-wide access
4. **Error UI**: Better error presentation with Tailwind styling
5. **Disabled states**: Prevents interaction during loading

### Pattern 2: Creating Custom Hooks for API Operations

Let's create reusable hooks that encapsulate API logic with proper error handling and loading states.

**File**: `src/hooks/useApiClient.ts`

```tsx
import { useState, useCallback } from 'react';

/**
 * Generic hook for handling API calls with loading and error states
 *
 * Usage:
 * const { execute, isLoading, error } = useApiCall();
 * const result = await execute(() => apiClient.someMethod());
 */
export function useApiCall<T>() {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<Error | null>(null);

  const execute = useCallback(async (apiCall: () => Promise<T>): Promise<T | null> => {
    setIsLoading(true);
    setError(null);

    try {
      const result = await apiCall();
      return result;
    } catch (err) {
      const error = err instanceof Error ? err : new Error('An unknown error occurred');
      setError(error);
      console.error('API call failed:', error);
      return null;
    } finally {
      setIsLoading(false);
    }
  }, []);

  const reset = useCallback(() => {
    setError(null);
    setIsLoading(false);
  }, []);

  return { execute, isLoading, error, reset };
}

/**
 * Hook for mutations (create, update, delete operations)
 * Provides additional success state tracking
 */
export function useApiMutation<TInput, TOutput>() {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<Error | null>(null);
  const [isSuccess, setIsSuccess] = useState(false);

  const mutate = useCallback(
    async (apiCall: (input: TInput) => Promise<TOutput>): Promise<(input: TInput) => Promise<TOutput | null>> => {
      return async (input: TInput) => {
        setIsLoading(true);
        setError(null);
        setIsSuccess(false);

        try {
          const result = await apiCall(input);
          setIsSuccess(true);
          return result;
        } catch (err) {
          const error = err instanceof Error ? err : new Error('An unknown error occurred');
          setError(error);
          console.error('API mutation failed:', error);
          return null;
        } finally {
          setIsLoading(false);
        }
      };
    },
    []
  );

  const reset = useCallback(() => {
    setError(null);
    setIsLoading(false);
    setIsSuccess(false);
  }, []);

  return { mutate, isLoading, error, isSuccess, reset };
}
```

**File**: `src/hooks/useUserOperations.ts`

```tsx
import { useCallback } from 'react';
import { apiClient } from '../api/client';
import { useUser } from '../components/context/userContext';
import { useApiCall } from './useApiClient';
import type { UpdateUserRequest } from '../services/web-api-client';

/**
 * Hook that provides user-related operations with built-in state management
 * Automatically updates UserContext when operations succeed
 */
export function useUserOperations() {
  const { user, setUser } = useUser();
  const { execute, isLoading, error } = useApiCall();

  /**
   * Fetch and update current user info
   */
  const refreshUser = useCallback(async () => {
    const userInfo = await execute(() => apiClient.auth_GetUserInfo());
    if (userInfo) {
      setUser(userInfo);
    }
    return userInfo;
  }, [execute, setUser]);

  /**
   * Update user profile
   */
  const updateProfile = useCallback(async (updates: UpdateUserRequest) => {
    const updatedUser = await execute(() => apiClient.user_UpdateProfile(updates));
    if (updatedUser) {
      setUser(updatedUser);
    }
    return updatedUser;
  }, [execute, setUser]);

  return {
    user,
    refreshUser,
    updateProfile,
    isLoading,
    error
  };
}
```

### Pattern 3: Using Context in Components

Here's how to use the UserContext and custom hooks in your Dashboard:

**File**: `src/components/Dashboard.tsx`

```tsx
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from './context/userContext';
import { useUserOperations } from '../hooks/useUserOperations';
import { AuthService } from '../auth/auth-service';
import { Button, Card } from './ui';
import ThemeToggle from './ThemeToggle';

const Dashboard = () => {
  const { user } = useUser();
  const { refreshUser, isLoading } = useUserOperations();
  const navigate = useNavigate();

  // Fetch user data on mount if not already loaded
  useEffect(() => {
    if (!user) {
      refreshUser();
    }
  }, [user, refreshUser]);

  const handleLogout = () => {
    AuthService.logout();
    navigate('/login');
  };

  if (isLoading) {
    return (
      <div className="min-h-screen bg-surface-light dark:bg-surface-dark flex items-center justify-center">
        <div className="text-content-light dark:text-content-dark text-lg">
          Loading...
        </div>
      </div>
    );
  }

  if (!user) {
    return (
      <div className="min-h-screen bg-surface-light dark:bg-surface-dark flex items-center justify-center">
        <Card className="max-w-md">
          <h2 className="text-xl font-semibold mb-4 text-content-light dark:text-content-dark">
            Please log in
          </h2>
          <Button variant="primary" onClick={() => navigate('/login')}>
            Go to Login
          </Button>
        </Card>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-surface-light dark:bg-surface-dark text-content-light dark:text-content-dark">
      {/* Header */}
      <header className="bg-white dark:bg-gray-800 shadow-sm border-b border-gray-200 dark:border-gray-700">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4 flex justify-between items-center">
          <h1 className="text-2xl font-bold">Dashboard</h1>
          <div className="flex items-center gap-4">
            <ThemeToggle />
            <Button variant="secondary" onClick={handleLogout}>
              Logout
            </Button>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {/* User Info Card */}
          <Card className="col-span-1">
            <h2 className="text-xl font-semibold mb-4">Welcome Back!</h2>
            <div className="space-y-2 text-sm">
              <p className="text-gray-600 dark:text-gray-400">
                <span className="font-medium">Email:</span> {user.email}
              </p>
              {user.firstName && (
                <p className="text-gray-600 dark:text-gray-400">
                  <span className="font-medium">Name:</span> {user.firstName} {user.lastName}
                </p>
              )}
            </div>
          </Card>

          {/* Add more dashboard content here */}
        </div>
      </main>
    </div>
  );
};

export default Dashboard;
```

**Key patterns demonstrated**:
1. **Conditional rendering**: Different UI for loading, no user, and authenticated states
2. **Auto-refresh**: Fetches user data if not in context
3. **Clean logout**: Uses AuthService to clear tokens and navigate
4. **Type-safe access**: TypeScript ensures you can't access undefined user properties
5. **Responsive layout**: Tailwind grid system for different screen sizes

---

## Part 3: Structuring Context Architecture for Scale

### When to Create New Contexts

**Create separate contexts when**:
- Data has different lifecycles (e.g., theme vs user vs notifications)
- Different parts of the app need different subsets of data
- You want to optimize re-renders (smaller contexts = fewer re-renders)
- Data is logically independent

**Don't create contexts for**:
- Simple prop drilling 1-2 levels deep (just pass props)
- Data that changes very frequently (use local state or a state management library)
- Computed values (derive them in components instead)

### Recommended Context Structure for Your App

**File**: `src/components/context/AppProviders.tsx`

```tsx
import { type ReactNode } from 'react';
import { UserProvider } from './userContext';
import { NotificationProvider } from './notificationContext';
// Import other providers as you create them

/**
 * Centralized provider composition
 * This makes it easy to add/remove providers and ensures correct nesting order
 */
export const AppProviders = ({ children }: { children: ReactNode }) => {
  return (
    <UserProvider>
      <NotificationProvider>
        {/* Add more providers here as needed */}
        {children}
      </NotificationProvider>
    </UserProvider>
  );
};
```

### Example: Notification Context

Here's a complete example of another context you might need:

**File**: `src/components/context/notificationContext.tsx`

```tsx
import { createContext, useContext, useState, useCallback, type ReactNode } from 'react';

export interface Notification {
  id: string;
  type: 'success' | 'error' | 'warning' | 'info';
  message: string;
  duration?: number; // milliseconds, undefined = don't auto-dismiss
}

interface NotificationContextType {
  notifications: Notification[];
  addNotification: (notification: Omit<Notification, 'id'>) => void;
  removeNotification: (id: string) => void;
  clearAll: () => void;
}

const NotificationContext = createContext<NotificationContextType | null>(null);

export const NotificationProvider = ({ children }: { children: ReactNode }) => {
  const [notifications, setNotifications] = useState<Notification[]>([]);

  const addNotification = useCallback((notification: Omit<Notification, 'id'>) => {
    const id = `notification-${Date.now()}-${Math.random()}`;
    const newNotification: Notification = { ...notification, id };

    setNotifications(prev => [...prev, newNotification]);

    // Auto-dismiss if duration is specified
    if (notification.duration) {
      setTimeout(() => {
        removeNotification(id);
      }, notification.duration);
    }
  }, []);

  const removeNotification = useCallback((id: string) => {
    setNotifications(prev => prev.filter(n => n.id !== id));
  }, []);

  const clearAll = useCallback(() => {
    setNotifications([]);
  }, []);

  return (
    <NotificationContext.Provider
      value={{ notifications, addNotification, removeNotification, clearAll }}
    >
      {children}
    </NotificationContext.Provider>
  );
};

export const useNotifications = () => {
  const context = useContext(NotificationContext);
  if (!context) {
    throw new Error('useNotifications must be used within a NotificationProvider');
  }
  return context;
};
```

**Usage in components**:

```tsx
import { useNotifications } from './context/notificationContext';

function SomeComponent() {
  const { addNotification } = useNotifications();

  const handleAction = async () => {
    try {
      await someApiCall();
      addNotification({
        type: 'success',
        message: 'Action completed successfully!',
        duration: 3000
      });
    } catch (error) {
      addNotification({
        type: 'error',
        message: 'Something went wrong. Please try again.',
        duration: 5000
      });
    }
  };

  return <button onClick={handleAction}>Do Something</button>;
}
```

### Folder Structure Recommendation

Here's how I recommend organizing your contexts and related code:

```
src/
├── components/
│   ├── context/
│   │   ├── userContext.tsx           # User authentication state
│   │   ├── notificationContext.tsx   # Toast notifications
│   │   ├── themeContext.tsx          # Dark/light theme
│   │   └── AppProviders.tsx          # Provider composition
│   ├── ui/                            # Reusable UI components
│   │   ├── Button.tsx
│   │   ├── Card.tsx
│   │   ├── Input.tsx
│   │   └── index.ts
│   ├── Login.tsx
│   ├── Dashboard.tsx
│   └── ...
├── hooks/
│   ├── useApiClient.ts                # Generic API call hooks
│   ├── useUserOperations.ts           # User-specific operations
│   └── ...
├── services/
│   ├── web-api-client.ts              # NSwag generated
│   ├── authenticated-api-client.ts    # Your wrapper
│   └── api-client-base.ts
├── auth/
│   ├── auth-service.ts
│   └── jwt/
│       └── jwt-manager.ts
└── types/
    └── index.ts                        # Shared type definitions
```

### Best Practices Summary

**Context Design**:
1. Keep contexts focused on a single concern
2. Use TypeScript interfaces for type safety
3. Always include the error check in your custom hooks
4. Consider making contexts read-only (only expose setters through specific functions)

**API Integration**:
1. Create custom hooks that wrap API calls with loading/error states
2. Update context immediately after successful API operations
3. Handle errors gracefully with user-friendly messages
4. Use the notification context for feedback

**Performance Optimization**:
1. Use `useCallback` for functions passed in context value
2. Consider splitting large contexts into smaller ones
3. Use `useMemo` for computed values in context
4. Don't put rapidly changing values in context (use local state instead)

**Error Handling**:
1. Always wrap API calls in try/catch
2. Provide loading states for better UX
3. Show user-friendly error messages
4. Log errors for debugging

---

## Complete Integration Example

Here's how everything fits together in your main App file:

**File**: `src/main.tsx`

```tsx
import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import { AppProviders } from './components/context/AppProviders';
import App from './App';
import './index.css';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <BrowserRouter>
      <AppProviders>
        <App />
      </AppProviders>
    </BrowserRouter>
  </React.StrictMode>
);
```

This wraps your entire app with all necessary providers, ensuring that `useUser()`, `useNotifications()`, and other context hooks work throughout your application.

---

## Summary

You've built a solid foundation with the UserContext pattern. The key takeaways are:

1. **Context = Data Channel**: Creates a way to pass data without prop drilling
2. **Provider = State Manager**: Holds state and handles side effects
3. **Custom Hook = Access Point**: Provides type-safe access with error handling
4. **Separation is Key**: Keep API logic in hooks, UI in components, and state in contexts
5. **Scale Thoughtfully**: Create focused contexts, compose them in AppProviders

This architecture will scale well as your application grows, maintaining type safety and code organization throughout.