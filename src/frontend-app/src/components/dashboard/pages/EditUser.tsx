import { useNavigate, useParams } from "react-router-dom";
import { Card, Button, Input, Select } from "../../ui";
import { LoadingSpinner } from "../../ui/LoadingSpinner";
import { ErrorDisplay } from "../../ui/ErrorDisplay";
import { useUser } from "../../hooks/useUser";
import { useState, useEffect } from "react";
import { useApiMutation } from "../../hooks/useApiClient";
import { AssignmentCreateRequest, UserInfoResponse } from "../../../services/web-api-client";
import { apiClient } from "../../../api/client";

export default function EditUser() {
    const { userId } = useParams();
    const navigate = useNavigate();
    const { user, isLoading, error } = useUser(userId);
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [email, setEmail] = useState("");
    const [role, setRole] = useState("");
    const { mutate, isLoading: isMutating, error: mutationError } = useApiMutation((payload: UserInfoResponse) => apiClient.auth_UpdateUserInfo(payload));

    useEffect(() => {
        if (user) {
            setFirstName(user.firstName ?? "");
            setLastName(user.lastName ?? "");
            setEmail(user.email ?? "");
            setRole(user.role ?? "");
        }
    }, [user]);

    const onSubmit: React.FormEventHandler<HTMLFormElement> = async (e) => {
        e.preventDefault();
        console.log("Submitting user edit form");
        const payload: UserInfoResponse = {
            id: userId!,
            firstName,
            lastName,
            role,
            email,
        };
        await mutate(payload);
        navigate("/dashboard/administration");
    };

    if (isLoading) return <LoadingSpinner />;
    if (error) return <ErrorDisplay error={error} />;
    if (mutationError) return <ErrorDisplay error={mutationError} />;

    if (!userId) {
        return (
            <Card className="max-w-2xl">
                <p className="text-gray-600 dark:text-gray-300">No user selected.</p>
            </Card>
        );
    }

    if (!user) {
        return (
            <Card className="max-w-2xl">
                <p className="text-gray-600 dark:text-gray-300">User not found.</p>
                <Button variant="secondary" className="mt-4" onClick={() => navigate(-1)}>
                    Go back
                </Button>
            </Card>
        );
    }

    return (
        <div>
            <div className="flex items-center justify-between mb-6">
                <div>
                    <p className="text-sm text-gray-500">Editing user</p>
                    <h1 className="text-3xl font-bold">
                        {user.firstName || user.lastName
                            ? `${user.firstName ?? ""} ${user.lastName ?? ""}`.trim()
                            : user.email ?? "Unknown user"}
                    </h1>
                </div>
            </div>
            <Card className="max-w-2xl">
                <form className="space-y-4" onSubmit={onSubmit}>
                    <Input
                        id="firstName"
                        label="First name"
                        name="firstName"
                        value={firstName}
                        onChange={(event) => setFirstName(event.target.value)}
                    />
                    <Input
                        id="lastName"
                        label="Last name"
                        name="lastName"
                        value={lastName}
                        onChange={(event) => setLastName(event.target.value)}
                    />
                    <Select
                        id="role"
                        name="role"
                        label="Role"
                        value={role}
                        onChange={(e) => setRole(e.target.value)}
                        required
                    >
                        <option value="Administrator">Administrator</option>
                        <option value="User">User</option>
                        <option value="Dispatcher">Dispatcher</option>
                    </Select>
                    <Input
                        id="email"
                        label="Email"
                        name="email"
                        type="email"
                        value={email}
                        onChange={(event) => setEmail(event.target.value)}
                    />
                    <div className="pt-4 flex gap-3">
                        <Button
                            type="submit"
                            variant="primary"
                            disabled={isMutating}
                            isLoading={isMutating}>
                            Save changes
                        </Button>
                        <Button type="button" variant="secondary" onClick={() => navigate(-1)}>
                            Cancel
                        </Button>
                    </div>
                </form>
            </Card>
        </div>
    );
}
