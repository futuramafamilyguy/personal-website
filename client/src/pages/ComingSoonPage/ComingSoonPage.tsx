import styles from "./ComingSoonPage.module.css";

const ComingSoonPage: React.FC = () => (
  <div className={styles.comingSoonPage}>
    <div className={styles.contentArea}>
      <div className={styles.left}>
        <div className={styles.textBox}>
          <h4>Coming Soon</h4>
          <br />
          <p>
            Following industry standards I have decided to release the website
            incomplete without all the features I had promised. They will
            eventually be implemented making this the biggest comeback in the
            industry.
          </p>
          <p>
            In the meantime, keep an eye on this page for what's to come or
            check out the{" "}
            <a
              href="https://github.com/futuramafamilyguy/personal-website"
              target="_blank"
              className={styles.externalLink}
            >
              GitHub repo
            </a>{" "}
            to view my progress. If I've been diligent, this page should be
            empty and you are most welcome to enjoy this picture of Lady Bird
            thrifting with her mum in Sacramento.
          </p>
          <p>
            <span style={{ fontStyle: "italic" }}>
              Last updated on 5/8/2024
            </span>
          </p>
        </div>
      </div>
      <div className={styles.right}>
        <div className={styles.textBox}>
          <h5>Blog</h5>
          <p>
            A space dedicated to my hot takes on media. Here are some sneak
            peeks:{" "}
            <span style={{ fontStyle: "italic" }}>
              "Shawshank Redemption is one of the best pictures released in the
              last 100 years"
            </span>
            ,{" "}
            <span style={{ fontStyle: "italic" }}>
              "Martin Scorsese is a hidden gem of a director"
            </span>
            ,{" "}
            <span style={{ fontStyle: "italic" }}>
              "Sick of a lack of original content from Hollywood"
            </span>
            .
          </p>
        </div>
        <div className={styles.textBox}>
          <h5>More session data</h5>
          <p>
            As of now, the{" "}
            <a
              href="https://allenmaygibson.com/sessions"
              target="_blank"
              className={styles.externalLink}
            >
              sesssions page
            </a>{" "}
            only displays session data for the newest releases of the current
            week. This means pictures that have debuted in the current week
            won't be present in the following weeks even if there are still
            sessions for it.
          </p>
          <p>
            An option to view session data of the remaining pictures will be
            added to the page.
          </p>
        </div>
      </div>
    </div>
  </div>
);

export default ComingSoonPage;
