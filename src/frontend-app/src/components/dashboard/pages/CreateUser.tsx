import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Card, Button, Input, Select } from "../../ui";
import { useApiMutation } from "../../hooks/useApiClient";
import { apiClient } from "../../../api/client";
import type { CreateUserRequest, UserInfoResponse } from "../../../services/web-api-client";

const initialForm: CreateUserRequest = {
    email: "",
    password: "",
    firstName: "",
    lastName: "",
    role: "User"
};

export default function CreateUser() {
    const navigate = useNavigate();
    const [formValues, setFormValues] = useState<CreateUserRequest>(initialForm);
    const { mutate, isLoading, error, isSuccess, reset } = useApiMutation<CreateUserRequest, UserInfoResponse>((payload) =>
        apiClient.user_CreateUser(payload)
    );

    const handleChange = (field: keyof CreateUserRequest) => (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        if (isSuccess) {
            reset();
        }
        setFormValues((prev) => ({
            ...prev,
            [field]: event.target.value
        }));
    };

    const handleSubmit: React.FormEventHandler<HTMLFormElement> = async (event) => {
        event.preventDefault();
        const result = await mutate(formValues);
        if (result) {
            setFormValues(initialForm);
        }
    };

    return (
        <div>
            <div className="flex items-center justify-between mb-6">
                <div>
                    <p className="text-sm text-gray-500">Administrator tools</p>
                    <h1 className="text-3xl font-bold">Add New User</h1>
                </div>
            </div>

            <Card className="max-w-2xl">
                <form onSubmit={handleSubmit} className="space-y-4">
                    <Input
                        id="email"
                        name="email"
                        label="Email"
                        type="email"
                        autoComplete="email"
                        value={formValues.email ?? ""}
                        onChange={handleChange("email")}
                        required
                        placeholder="person@example.com"
                    />

                    <Input
                        id="password"
                        name="password"
                        label="Temporary Password"
                        type="password"
                        autoComplete="new-password"
                        value={formValues.password ?? ""}
                        onChange={handleChange("password")}
                        required
                        placeholder="Set an initial password"
                    />

                    <Input
                        id="firstName"
                        name="given-name"
                        label="First Name"
                        autoComplete="given-name"
                        value={formValues.firstName ?? ""}
                        onChange={handleChange("firstName")}
                        required
                        placeholder="Jane"
                    />

                    <Input
                        id="lastName"
                        name="family-name"
                        label="Last Name"
                        autoComplete="family-name"
                        value={formValues.lastName ?? ""}
                        onChange={handleChange("lastName")}
                        required
                        placeholder="Doe"
                    />

                    <Select
                        id="role"
                        name="role"
                        label="Role"
                        value={formValues.role ?? "User"}
                        onChange={handleChange("role")}
                        required
                    >
                        <option value="Administrator">Administrator</option>
                        <option value="Dispatcher">Dispatcher</option>
                        <option value="User">User</option>
                    </Select>

                    {error && (
                        <div className="rounded-md bg-red-50 p-3 text-sm text-red-700">
                            {error.message}
                        </div>
                    )}

                    {isSuccess && !error && (
                        <div className="rounded-md bg-green-50 p-3 text-sm text-green-700">
                            User created successfully. Share the temporary password so they can sign in and update their profile.
                        </div>
                    )}

                    <div className="pt-4 flex gap-3">
                        <Button type="submit" variant="primary" disabled={isLoading}>
                            {isLoading ? "Creating..." : "Create User"}
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
