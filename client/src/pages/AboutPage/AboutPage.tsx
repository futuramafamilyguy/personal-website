import { Link } from "react-router-dom";

import { useIsMobile } from "../../hooks/useIsMobile";
import styles from "./AboutPage.module.css";

function AboutPage() {
  const isMobile = useIsMobile();

  const pages = [
    {
      title: "letterbox",
      subtitle: "i single handedly keep the cinema industry alive",
      path: "/letterbox",
    },
    { title: "blog", subtitle: "hottest takes on the web", path: "/blog" },
    {
      title: "operation kino",
      subtitle: "movie showtimes in nz",
      path: "/operation-kino",
    },
    {
      title: "github",
      subtitle: "my colleagues have seen much worse from me",
      path: "https://github.com/futuramafamilyguy/personal-website",
    },
  ];

  return (
    <div className={styles.aboutPage}>
      <div className={styles.centeredContainer}>
        <div className={styles.descriptionBox}>
          <h3>kia ora *~*</h3>
          <h5>
            i'm a frequent moviegoer based in south auckland, new zealand—home
            of big ben pies and lucy lawless xena: warrior princess{" "}
            <span className={styles.smallText}>i love u lucy</span> thanks for
            visiting *~*
          </h5>
          {!isMobile && (
            <h5>
              <i>
                i was watching you out there before. i've never seen you look so
                sexy
              </i>
            </h5>
          )}

          {isMobile && (
            <div className={styles.pageList}>
              {pages.map((page) => (
                <div key={page.path} className={styles.pageItem}>
                  <Link className={styles.pageTitle} to={page.path}>
                    <h2 className={styles.pageTitle}>{page.title}</h2>
                  </Link>
                  <h5 className={styles.pageSubtitle}>{page.subtitle}</h5>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

export default AboutPage;
