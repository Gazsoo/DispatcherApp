import { Link, useNavigate } from "react-router-dom";
import { useState } from "react";
import { Button, Card, Table, TableHeader, TableBody, TableRow, TableHead, TableCell } from "../../ui";
import { LoadingSpinner } from "../../ui/LoadingSpinner";
import { ErrorDisplay } from "../../ui/ErrorDisplay";
import { useUsers } from "../../hooks/useUsers";
import { useApiMutation } from "../../hooks/useApiClient";
import { apiClient } from "../../../api/client";

export default function Administrations() {
    const { users, isLoading, error, refetch } = useUsers();
    const navigate = useNavigate();
    const [deletingId, setDeletingId] = useState<string | null>(null);
    const { mutate: deleteUser, isLoading: isDeleting } = useApiMutation((id: string) => apiClient.user_DeleteUser(id));

    if (isLoading) {
        return <LoadingSpinner />;
    }

    if (error) {
        return <ErrorDisplay error={error} onRetry={refetch} />;
    }

    const getDisplayName = (firstName?: string | null, lastName?: string | null, fallback?: string | null) => {
        const first = firstName?.trim() ?? "";
        const last = lastName?.trim() ?? "";
        const fullName = `${first} ${last}`.trim();
        return fullName || fallback || "Unknown user";
    };


    const onDeleteUser = async (userId: string, userName: string) => {
        const confirmed = window.confirm(`Delete ${userName}?`);
        if (!confirmed) return;
        try {
            setDeletingId(userId);
            const result = await deleteUser(userId);
            if (result !== null) {
                await refetch();
            } else {
                window.alert("Failed to delete user.");
            }
        } finally {
            setDeletingId(null);
        }
    };

    return (
        <div>
            <div className="flex items-center justify-between mb-6 flex-wrap gap-3">
                <h1 className="text-3xl font-bold">Administration</h1>
                <Link to="/dashboard/administration/create">
                    <Button variant="primary">Add User</Button>
                </Link>
            </div>

            {users.length === 0 ? (
                <Card className="p-6 text-center text-gray-500 w-auto max-w-none">
                    No users found.
                </Card>
            ) : (
                <Card className="p-0 overflow-hidden w-auto max-w-none">
                    <div className="overflow-x-auto">
                        <Table>
                            <TableHeader>
                                <TableRow>
                                    <TableHead>Name</TableHead>
                                    <TableHead>Email</TableHead>
                                    <TableHead>Role</TableHead>
                                    <TableHead className="w-24"> </TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {users.map((user) => (
                                    <TableRow
                                        key={user.id ?? user.email}
                                        className="cursor-pointer hover:bg-accent/5 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-accent transition-colors"
                                        role="button"
                                        tabIndex={0}
                                        onClick={() => user.id && navigate(`/dashboard/administration/${user.id}`)}
                                        onKeyDown={(event) => {
                                            if ((event.key === "Enter" || event.key === " ") && user.id) {
                                                event.preventDefault();
                                                navigate(`/dashboard/administration/${user.id}`);
                                            }
                                        }}
                                    >
                                        <TableCell className="font-medium text-content-light dark:text-content-dark">
                                            {getDisplayName(user.firstName, user.lastName, user.email)}
                                        </TableCell>
                                        <TableCell className="text-gray-600 dark:text-gray-300">
                                            {user.email ?? "—"}
                                        </TableCell>
                                        <TableCell>
                                            <span className="inline-flex items-center rounded-full bg-accent/10 px-3 py-1 text-xs font-semibold text-accent">
                                                {user.role ?? "—"}
                                            </span>
                                        </TableCell>
                                        <TableCell>
                                            <div className="flex items-center gap-2">
                                                <Button
                                                    variant="dangerSubtle"
                                                    size="sm"
                                                    type="button"
                                                    className="w-auto"
                                                    isLoading={isDeleting && deletingId === user.id}
                                                    disabled={isDeleting && deletingId === user.id}
                                                    onClick={(e) => {
                                                        e.preventDefault();
                                                        e.stopPropagation();
                                                        onDeleteUser(user.id!, getDisplayName(user.firstName, user.lastName, user.email));
                                                    }}
                                                >
                                                    Delete
                                                </Button>
                                            </div>
                                        </TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </div>
                </Card>
            )}
        </div>
    );
}
