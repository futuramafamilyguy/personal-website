import { useEffect, useState } from "react";

import QstFm from "../../components/QstFm/QstFm";
import styles from "./QstFmPage.module.css";

interface Track {
  title: string;
  artist: string;
  url: string;
  original: string;
}

interface Segment {
  cover: string;
  intro: string;
  playlist: Track[];
}

const turtleDoves: Segment = {
  cover: "https://cdn.allenmaygibson.com/images/static/turtle-doves.jpg",
  intro:
    "good evening, queen street. this next one goes out to our loves — the ones we fight with, the ones we dance with, and the ones we still miss. ohia noa atu. my longing for you is unrestricted",
  playlist: [],
};

function QstFmPage() {
  const [segment, setSegment] = useState<Segment>(turtleDoves);

  return (
    <div
      className={styles.qstFmPage}
      style={{
        ["--bg-image" as any]: `url(${segment.cover})`,
      }}
    >
      <div className={styles.centeredContainer}>
        <div className={styles.descriptionBox}>
          <h5>
            <i>{segment.intro}</i>
          </h5>
        </div>
      </div>
    </div>
  );
}

export default QstFmPage;
