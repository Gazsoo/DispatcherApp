import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { Card, Button, Input, Select } from "../../ui";
import { useUser } from "../../context/userContext";
import { useApiCall, useApiMutation } from "../../hooks/useApiClient";
import { apiClient } from "../../../api/client";
import { AssignmentCreateRequest, AssignmentStatus, type GetAllUsersResponse, type UserInfoResponse } from "../../../services/web-api-client";

export default function CreateAssignment() {
    const navigate = useNavigate();
    const { mutate, isLoading: isMutating, error: mutationError } = useApiMutation((payload: AssignmentCreateRequest) => apiClient.assignment_CreateAssignment(payload));
    const { execute: loadUsers, isLoading: isUsersLoading, error: usersError } = useApiCall<GetAllUsersResponse>();
    const { user } = useUser();

    const [isSubmitting, setIsSubmitting] = useState(false);
    const [submitError, setSubmitError] = useState<string | null>(null);
    const [status, setStatus] = useState<AssignmentStatus>("Pending");
    const [assigneeIds, setAssigneeIds] = useState<string[]>([]);
    // Helper to parse the value from an <input type="datetime-local"> into a local Date
    const parseLocalDateTime = (value: string): Date => {
        // value format example: "2025-11-03T14:30"
        const [datePart, timePart] = value.split('T');
        const [year, month, day] = datePart.split('-').map(Number);
        const [hour, minute] = (timePart || '00:00').split(':').map(Number);
        return new Date(year, (month - 1), day, hour, minute);
    };
    const [users, setUsers] = useState<UserInfoResponse[]>([]);

    useEffect(() => {
        const fetchUsers = async () => {
            const res = await loadUsers(() => apiClient.user_GetAllUsers());
            if (res?.users) setUsers(res.users);
        };
        fetchUsers();
    }, [loadUsers]);

    // Keep state as-is; generate options from a single source of truth
    const STATUSES: ReadonlyArray<AssignmentStatus> = [
        "Pending",
        "InProgress",
        "Completed",
        "Cancelled",
    ];

    const onSubmit: React.FormEventHandler<HTMLFormElement> = async (e) => {
        e.preventDefault();
        setIsSubmitting(true);
        setSubmitError(null);
        try {
            const form = e.currentTarget;
            const formData = new FormData(form);
            const title = String(formData.get("title") ?? "");
            const description = formData.get("description")?.toString() || undefined;
            const plannedRaw = formData.get("plannedTime")?.toString();
            const plannedTime = plannedRaw ? parseLocalDateTime(plannedRaw) : undefined;
            const payload: AssignmentCreateRequest = {
                name: title,
                description,
                assigneeIds,
                status,
                plannedTime,

            };
            await mutate(payload);
            console.debug("CreateAssignment payload:", payload);

            // Navigate back to assignments list on success
            navigate("/dashboard/assignments");
        } catch (err) {
            const msg = err instanceof Error ? err.message : String(err);
            setSubmitError(msg);
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <div>
            <div className="flex items-center justify-between mb-6">
                <h1 className="text-3xl font-bold">Create Assignment</h1>
            </div>
            <Card className="max-w-2xl">
                <form onSubmit={onSubmit} className="space-y-4">
                    <Input id="title" name="title" label="Title" placeholder="Enter assignment title" required />
                    <Input id="description" name="description" label="Description" placeholder="Short description" />
                    <Input id="plannedTime" name="plannedTime" type="datetime-local" label="Planned Time" />
                    <Select
                        id="assignees"
                        name="assignees"
                        label="Assignees"
                        multiple
                        value={assigneeIds}
                        onChange={(e) => setAssigneeIds(Array.from(e.currentTarget.selectedOptions).map(o => o.value))}
                        disabled={isUsersLoading}
                    >
                        {users.map(u => (
                            <option key={u.id} value={u.id}>{u.firstName || u.email} {u.lastName || ''}</option>
                        ))}
                    </Select>
                    {usersError && <p className="text-red-600 text-sm">Failed to load users.</p>}
                    <Select
                        id="status"
                        name="status"
                        label="Status"
                        value={status}
                        onChange={(e) => setStatus(e.target.value as AssignmentStatus)}
                        required
                    >
                        {STATUSES.map((s) => (
                            <option key={s} value={s}>
                                {s}
                            </option>
                        ))}
                    </Select>

                    {submitError && (
                        <p className="text-red-600 text-sm">{submitError}</p>
                    )}

                    <div className="pt-2 flex gap-3">
                        <Button type="submit" variant="primary" disabled={isSubmitting}>{isSubmitting ? "Creating..." : "Create"}</Button>
                        <Button type="button" variant="secondary" onClick={() => navigate(-1)}>Cancel</Button>
                    </div>
                </form>
            </Card>
        </div>
    );
}
