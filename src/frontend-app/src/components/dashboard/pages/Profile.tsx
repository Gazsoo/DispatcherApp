import { useState } from "react";
import { useUser } from "../../context/userContext";
import { apiClient } from "../../../api/client";
import { Button, Card, InfoField, Input } from "../../ui";
import { useApiCall } from "../../hooks/useApiClient";

export default function Profile() {
    const { user, setUser } = useUser();
    const [isEditing, setIsEditing] = useState(false);
    const [formData, setFormData] = useState({
        firstName: user?.firstName || '',
        lastName: user?.lastName || '',
    });
    const { execute, error, isLoading } = useApiCall();

    const handleSave = async (e: React.FormEvent) => {
        e.preventDefault();

        const result = await execute(() => apiClient.auth_UpdateUserInfo({
            ...user,
            firstName: formData.firstName,
            lastName: formData.lastName,
        }));

        if (result) {
            setUser({ ...user!, firstName: formData.firstName, lastName: formData.lastName });
            setIsEditing(false);
        }
    };

    const handleCancel = () => {
        setFormData({
            firstName: user?.firstName || '',
            lastName: user?.lastName || '',
        });
        setIsEditing(false);
    };

    if (!user) return null;

    const renderViewMode = () => (
        <>
            <div className="space-y-3">
                <InfoField label="First Name" value={user.firstName} />
                <InfoField label="Last Name" value={user.lastName} />
                <InfoField label="Email" value={user.email} placeholder="" />
                <InfoField
                    label="Email Status"
                    value={user.emailConfirmed ? '✓ Verified' : 'Pending verification'}
                />
                <InfoField
                    label="Two-Factor Auth"
                    value={user.twoFactorEnabled ? 'Enabled' : 'Disabled'}
                />
            </div>
            <Button variant="secondary" onClick={() => setIsEditing(true)} className="mt-4">
                Edit Profile
            </Button>
        </>
    );

    const renderEditMode = () => (
        <form onSubmit={handleSave} className="space-y-4">
            <Input
                id="firstName"
                label="First Name"
                type="text"
                value={formData.firstName}
                onChange={(e) => setFormData({ ...formData, firstName: e.target.value })}
            />
            <Input
                id="lastName"
                label="Last Name"
                type="text"
                value={formData.lastName}
                onChange={(e) => setFormData({ ...formData, lastName: e.target.value })}
            />
            <Input
                id="email"
                label="Email"
                type="email"
                value={user.email}
                disabled
                className="opacity-50"
            />
            <div className="flex gap-3 pt-4">
                <Button type="submit" variant="primary" isLoading={isLoading}>
                    Save Changes
                </Button>
                <Button type="button" variant="secondary" onClick={handleCancel}>
                    Cancel
                </Button>
            </div>
        </form>
    );

    return (
        <div className="max-w-2xl">
            <h1 className="text-3xl font-bold mb-6">Profile</h1>

            <Card className="p-6">
                <h2 className="text-xl font-semibold mb-6">Personal Information</h2>

                {error && (
                    <div className="mb-4 p-3 bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 rounded text-red-600 dark:text-red-400">
                        {error.message}
                    </div>
                )}

                {isEditing ? renderEditMode() : renderViewMode()}
            </Card>
        </div>
    );
}