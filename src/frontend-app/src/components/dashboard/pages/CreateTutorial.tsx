import { useNavigate } from "react-router-dom";
import { useState } from "react";
import { Card, Button, Input } from "../../ui";
import { useApiMutation } from "../../hooks/useApiClient";
import { apiClient } from "../../../api/client";
import type { CreateTutorialRequest, CreateTutorialResponse } from "../../../services/web-api-client";
import { useUser } from "../../context/userContext";
import { LoadingSpinner } from "../../ui/LoadingSpinner";
import { ErrorDisplay } from "../../ui/ErrorDisplay";

export default function CreateTutorial() {
    const { user } = useUser();
    const navigate = useNavigate();
    const [files, setFiles] = useState<File[]>([]);
    // Important: pass a bound function so `this` inside the generated client stays intact
    const { mutate, error, isLoading } = useApiMutation<CreateTutorialRequest, CreateTutorialResponse>((payload) => apiClient.tutorial_CreateTutorial(payload));
    const onFileChange: React.ChangeEventHandler<HTMLInputElement> = (e) => {
        const list = e.target.files;
        if (!list) return;
        setFiles(Array.from(list));
    };
    if (isLoading) return <LoadingSpinner />;
    if (error) return <ErrorDisplay error={error} />;
    const removeFile = (index: number) => {
        setFiles(prev => prev.filter((_, i) => i !== index));
    };

    const onSubmit: React.FormEventHandler<HTMLFormElement> = async (e) => {
        e.preventDefault();

        const form = e.currentTarget;
        const formData = new FormData(form);
        const title = String(formData.get("title") ?? "");
        const description = formData.get("description")?.toString() || undefined;

        const fileIds = (await Promise.all(
            files.map(async (file) => {
                const uploaded = await apiClient.files_PostFile({ data: file, fileName: file.name });
                return uploaded.id;
            })
        )).filter((id): id is number => typeof id === "number");

        const result = await mutate({
            title,
            description,
            filesId: fileIds,
            createdById: user?.id,
        });

        if (result) {
            navigate("/dashboard/tutorials");
        }
    };

    return (

        <div>

            <div className="flex items-center justify-between mb-6">
                <h1 className="text-3xl font-bold">Create Tutorial</h1>
            </div>
            <Card className="max-w-2xl">
                <form onSubmit={onSubmit} className="space-y-4">
                    <Input id="title" name="title" label="Title" placeholder="Enter tutorial title" required />
                    <Input id="description" name="description" label="Description" placeholder="Short description" />
                    <Input id="url" name="url" label="URL (optional)" placeholder="https://..." type="url" />

                    <div>
                        <label htmlFor="files" className="block text-sm font-medium mb-2">Files</label>
                        <input
                            id="files"
                            name="files"
                            type="file"
                            multiple
                            onChange={onFileChange}
                            className="block w-full text-sm file:mr-4 file:py-2 file:px-4 file:rounded-md file:border-0 file:text-sm file:font-semibold file:bg-accent file:text-white hover:file:bg-accent-dark"
                        />
                        {files.length > 0 && (
                            <ul className="mt-3 space-y-2 text-sm">
                                {files.map((f, i) => (
                                    <li key={i} className="flex items-center justify-between rounded-md border px-3 py-2">
                                        <span className="truncate mr-3">{f.name}</span>
                                        <Button type="button" variant="secondary" onClick={() => removeFile(i)}>Remove</Button>
                                    </li>
                                ))}
                            </ul>
                        )}
                    </div>

                    <div className="pt-2 flex gap-3">
                        <Button type="submit" variant="primary" disabled={isLoading}>{isLoading ? "Creating..." : "Create"}</Button>
                        <Button type="button" variant="secondary" onClick={() => navigate(-1)}>Cancel</Button>
                    </div>

                </form>
            </Card>
        </div>
    );
}
