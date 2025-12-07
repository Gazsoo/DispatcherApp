import { useParams } from "react-router";
import { useTutorial } from "../../hooks/useTutorials";
import { LoadingSpinner } from "../../ui/LoadingSpinner";
import { ErrorDisplay } from "../../ui/ErrorDisplay";
import FileLink from "../components/FileLink";
import { Button, Card, InfoField } from "../../ui";
import { useState } from "react";

export default function TutorialDetails() {
    const { tutorialId } = useParams();
    const { error, tutorial, isLoading } = useTutorial(tutorialId ? parseInt(tutorialId) : undefined);
    const [isEditing, setIsEditing] = useState(false);

    if (isLoading) return <LoadingSpinner />;
    if (error) return <ErrorDisplay error={error} />;

    return (
        <div>

            <h1 className="text-3xl font-bold mb-6">Tutorial Details for {tutorial?.title ?? "Unknown"}</h1>
            <Card>

                {tutorial && <>
                    <div className="space-y-3">
                        <InfoField label="Title" value={tutorial.title} />
                        <InfoField label="Description" value={tutorial.description} />
                        <InfoField
                            label="Created At"
                            value={tutorial.createdAt ? new Date(tutorial.createdAt).toLocaleDateString() + " " + new Date(tutorial.createdAt).toLocaleTimeString() : ''}
                            placeholder="" />
                    </div>

                </>
                }
                {tutorial?.files && tutorial.files.length > 0 && <h2 className="text-xl font-semibold mt-6 mb-4">Files</h2>}
                {tutorial && tutorial.files?.map((file) => (
                    <FileLink key={file.id} file={file}></FileLink>
                ))}
                <Button variant="secondary" onClick={() => setIsEditing(true)} className="mt-4">
                    Edit Tutorial
                </Button>
            </Card>
        </div>
    );
}
