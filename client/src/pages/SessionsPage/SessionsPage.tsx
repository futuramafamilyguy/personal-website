import RegionsContainer from "../../components/RegionsContainer/RegionsContainer";
import { RegionProvider } from "../../contexts/RegionContext";
import styles from "./SessionsPage.module.css";

function LetterboxcPage() {
  const calculateNextRun = () => {
    const now = new Date();
    const nextSunday = new Date(now);

    nextSunday.setDate(now.getDate() + ((7 - now.getDay()) % 7));
    nextSunday.setHours(12, 0, 0, 0);

    const difference = nextSunday.getTime() - now.getTime();

    const days = Math.floor(difference / (1000 * 60 * 60 * 24));
    const hours = Math.floor(
      (difference % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)
    );

    if (days === 0 && hours === 0) {
      return `less than an hour`;
    } else if (days === 0) {
      return `${hours} hour${hours > 1 ? "s" : ""}`;
    } else {
      return `${days} day${days > 1 ? "s" : ""}`;
    }
  };
  return (
    <>
      <div className={styles.sessionsPage}>
        <div className={styles.textBox}>
          <h4>NZ Picture Sessions</h4>
          <br />
          <p>
            Latest pictures that have been released in NZ cinemas this week,
            including re-releases of older classics. Updated every Sunday at
            12pm NZT (next run in <b>{calculateNextRun()}</b>).
          </p>
        </div>
        <RegionProvider>
          <RegionsContainer />
        </RegionProvider>
      </div>
    </>
  );
}

export default LetterboxcPage;
