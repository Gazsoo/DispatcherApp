import { LoadingSpinner } from "../../ui/LoadingSpinner";
import { ErrorDisplay } from "../../ui/ErrorDisplay";
import { useTutorials } from "../../hooks/useTutorials";
import { Outlet, useParams } from "react-router-dom";
import { Card, Button } from "../../ui";
import { Link } from "react-router-dom";
import { TutorialCard } from "./TutorialCard";

export default function Tutorials() {
    const { tutorials, isLoading, error, refetch } = useTutorials();
    const { tutorialId } = useParams();

    if (tutorialId) {
        return <Outlet />;
    }

    if (isLoading) return <LoadingSpinner />;
    if (error) return <ErrorDisplay error={error} onRetry={refetch} />;

    return (
        <div>
            <div className="flex items-center justify-between mb-6">
                <h1 className="text-3xl font-bold">Tutorials</h1>
                <Link to="/dashboard/tutorials/create">
                    <Button variant="primary">Create Tutorial</Button>
                </Link>
            </div>

            {tutorials.length === 0 ? (
                <Card className="p-6 text-center text-gray-500">
                    No tutorials available yet.
                </Card>
            ) : (
                <div className="grid gap-4 md:grid-cols-2 xl:grid-cols-4">
                    {tutorials.map((tutorial) => (
                        <TutorialCard key={tutorial.id} tutorial={tutorial} />
                    ))}
                </div>
            )}
        </div>
    );
}