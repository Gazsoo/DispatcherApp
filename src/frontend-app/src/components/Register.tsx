import { useState } from "react";
import { Button, Card, Input } from "./ui";
import { useAuth } from "./hooks/useAuth";
import { useNavigate } from "react-router";

const Register = () => {

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [role, setRole] = useState('User');
    const { register, isLoading, error } = useAuth();
    const navigate = useNavigate();

    const handleRegister = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await register(email, password, firstName, lastName, role);
            navigate('/login', { state: { notice: 'Account created. Please check your email to verify your account, then sign in.' } });
        } catch {
            // error state already set by hook
        }
    };

    return (
        <div className="flex flex-col items-center justify-center min-h-screen p-2">
            <Card>
                <form onSubmit={handleRegister} className="space-y-6">
                    <Input
                        id="email"
                        name="email"
                        label="Email"
                        type="email"
                        value={email}
                        autoComplete="email"
                        onChange={(e) => setEmail(e.target.value)}
                        required
                        placeholder="your@email.com"
                    />

                    <Input
                        id="password"
                        name="password"
                        label="Password"
                        type="password"
                        value={password}
                        autoComplete="new-password"
                        onChange={(e) => setPassword(e.target.value)}
                        required
                        placeholder="••••••••"
                    />

                    <Input
                        id="firstName"
                        name="given-name"
                        label="First Name"
                        value={firstName}
                        autoComplete="given-name"
                        onChange={(e) => setFirstName(e.target.value)}
                        required
                        placeholder="John"
                    />

                    <Input
                        id="lastName"
                        name="family-name"
                        label="Last Name"
                        value={lastName}
                        autoComplete="family-name"
                        onChange={(e) => setLastName(e.target.value)}
                        required
                        placeholder="Doe"
                    />
                    <div>
                        <label htmlFor="role" className="block text-sm font-medium mb-2">Role</label>
                        <select
                            id="role"
                            name="role"
                            value={role}
                            onChange={(e) => setRole(e.target.value)}
                            required
                            className="w-full px-4 py-3 rounded-lg bg-surface-light dark:bg-surface-dark border border-surface-light-border dark:border-surface-dark-border focus:outline-none focus:ring-2 focus:ring-accent transition-all"
                        >
                            <option value="Administrator">Administrator</option>
                            <option value="User">User</option>
                            <option value="Dispatcher">Dispatcher</option>
                        </select>
                    </div>

                    {error && (
                        <div className="text-red-500 text-sm text-center">
                            {error}
                        </div>
                    )}

                    <Button type="submit" variant="primary" isLoading={isLoading}>
                        Register
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
    );
}
export default Register;