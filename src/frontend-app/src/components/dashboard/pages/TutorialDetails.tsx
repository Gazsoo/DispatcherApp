import { useParams } from "react-router";
import { useTutorial, useTutorialFile } from "../../hooks/useTutorials";
import { LoadingSpinner } from "../../ui/LoadingSpinner";
import { ErrorDisplay } from "../../ui/ErrorDisplay";

export default function TutorialDetails() {
    const { tutorialId } = useParams();
    const { error, tutorial, isLoading } = useTutorial(tutorialId ? parseInt(tutorialId) : 0);
    const { error: fileError, file: tutorialFile, isLoading: fileLoading } = useTutorialFile(
        tutorial?.id || 0,
        tutorial?.files?.[0].id || 0
    );

    if (isLoading || fileLoading) return <LoadingSpinner />;
    if (error) return <ErrorDisplay error={error} />;
    if (fileError) return <ErrorDisplay error={fileError} />;

    return (
        <div>
            <h1 className="text-3xl font-bold mb-6">Tutorial Details for {tutorialFile?.fileName ?? "Unknown"}</h1>
            {/* Tutorial details for tutorialId: {tutorialId} */}
        </div>
    );
}