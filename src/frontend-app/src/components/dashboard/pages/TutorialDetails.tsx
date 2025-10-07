import { useParams } from "react-router";
import { useTutorial, useTutorialFile } from "../../hooks/useTutorials";
import { LoadingSpinner } from "../../ui/LoadingSpinner";
import { ErrorDisplay } from "../../ui/ErrorDisplay";

export default function TutorialDetails() {
    const { tutorialId } = useParams();
    const { error, tutorial, isLoading } = useTutorial(tutorialId ? parseInt(tutorialId) : undefined);

    if (isLoading) return <LoadingSpinner />;
    if (error) return <ErrorDisplay error={error} />;

    return (
        <div>
            <h1 className="text-3xl font-bold mb-6">Tutorial Details for {tutorial?.title ?? "Unknown"}</h1>
            {tutorial && tutorial.files?.map((file) => (
                <div key={file.id} className="mb-4 p-4 border border-gray-200 rounded-lg">
                    <h2 className="text-xl font-semibold mb-2">{file.fileName}</h2>
                </div>
            ))}
        </div>
    );
}